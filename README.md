# MyLab.Smev

[![NuGet Version and Downloads count](https://buildstats.info/nuget/MyLab.Smev)](https://www.nuget.org/packages/MyLab.Smev)

```
Платформа: .NET 6
```

Ознакомьтесь с последними изменениями в [журнале изменений](/CHANGELOG.md).

## Обзор

Частичная реализация HTTP клиента для СМЭВ 3 (версии схем 1.1) с поддержкой подписи XML средствами СКЗИ КРИПТО-ПРО для Linux

Реализованные методы:

1. SendRequest (Отправка запроса)
2. GetResponse (Получение ответа из очереди входящих ответов)
3. Ack (Подтверждение сообщения)

## Конфигурация

### Содержание конфигурации

Узел конфигурации по умолчанию - `Smev`.

Параметры конфигурации:

* `Url` - URL конечной точки СМЭВ сервиса;
* `CertThumbprint` - отпечаток (sha-1 хэш) сертификата, которым будут подписываться запросы.

Пример конфигурации:

```json
{
  "Smev": {
    "Url": "http://smev123.gosuslugi.ru/smev/v123/ws",
    "CertThumbprint": "******"
  }
}
```

### Применение конфигурации

Применение конфигурации в коде может осуществляться одним из следующих методов:

* применение объекта конфигурации

  ```C#
  IConfiguration config = ... ;
  IServiceCollection services = ... ;
  
  // C именем узла конфигурации по умолчанию
  services.ConfigureSmev3Client(config); 
  
  // C кастомным именем узла конфигурации 
  services.ConfigureSmev3Client(config, "smev3") 
  ```

* инициализация в делегате
  ```c#
  IServiceCollection services = ... ;
  
  services.ConfigureSmev3Client(o =>
      {
          o.Url = "http://smev123.gosuslugi.ru/smev/v123/ws";
      });
  ```

Допускается комбинированное использование этих методов.

## Внедрение

Для внедрения клиента СМЭВ3 необходимо:

* добавить клиент в сервисы
  ```c#
  IServiceCollection services = ... ;
  
  services.AddSmev3Client()
  ```

* сконфигурировать клиент (см. [Конфигурация](#Конфигурация)) 
  ```c#
  IConfiguration config = ... ;
  IServiceCollection services = ... ;
  
  services.ConfigureSmev3Client(config);
  ```

* получить объект клиента для использования в целевом месте
  ```c#
  class MyService
  {
      public MyService(ISmev3Client smev3Client)
      {
          
      }
  }
  ```

## Применение

### Отправка бизнес-запроса

Данная операция подразумевает отправку запроса, специфичного для конкретного сервиса-получателя. Этот запрос оборачивается в служебный объект и отправляется клиентом. В ответ метод выдаёт объект, содержащий идентификатор, назначенный сообщению и ассоциированный с отправленным запросом.

Пример отправки бизнес-запроса:

```C#
ISmev3Client client = ...;

var bizReq = new MyBizRequest
{
    // Заполнение полей специфичного для сервиса-получателя запроса 
};

var reqCtx = new SendRequestExecutionContext<MyBizRequest>
{
	RequestData = bizReq
};

var resp = await client.SendRequestAsync(reqCtx);

// Идентификатор, назначенный сообщению и ассоциированный с ортправленным запросом
var requestId = resp.Data.MessageMetadata.MessageId;  
```

### Получение бизнес-ответа

Получение результата происходит по модели получения сообщений из очереди. Запрос получения ответа запрашивает очередное сообщение из очереди ответов.

При запросе получения бизнес-ответа необходимо указать имя и пространство имён XML-элемента бизнес ответа, который ожидается получить. 

Пример получения бизнес-ответа:

```C#
ISmev3Client client = ...;

// Имя xml-элемента ответа
const string localName = "Response";
// Пространство имён xml-элемента ответа
const string namespaceUri = "http://myhost.ru/chto-to-tam/blabla/v2";

var soapResp = await client.GetResponseAsync<Response>(new Uri(namespaceUri), localName);
var smevResp = resp.Data.ResponseMessage.Response;

if(smevResp != null)
{
    // Десериализованный бизнес-ответ
    var bizResponse = smevResp.SenderProvidedResponseData.MessagePrimaryContent.Content;
    // Идентификатор сообщения
    var responseMessageId = smevResp.MessageMetadata.MessageId;
    // Идентификатор сообщения, на которое был сформирован этот ответ
    var requestMessageId = smevResp.OriginalMessageId;
}
else
{
    // Очередь бизнес-ответов пуста
}
```

### Подтверждение получения

Для подтверждения получения бизнес-ответа, необходимо указать идентификатор сообщения, полученного в ответе на запрос получения бизнес-ответа.

Так же метод имеет входной параметр `accepted`, который должен иметь значения:

* `true` - если ЭП-СМЭВ прошла валидацию и сообщение передано ИС;
* `false` - если ЭП-СМЭВ отвергнута, и сообщение проигнорировано.

```C#
ISmev3Client client = ...;

try
{
    await client.AckAsync(requestId, accepted: true);
    
    // Если не было исключения, значит подтверждено успешно
}
catch(Smev3ClientException e) when (e.IsMessageNotFound())
{
    // В очереди ответов сообщение с указанным идентификатором не найдено 
}
```

### Обработка ошибок

Во время взаимодействия со СМЭВ, могут возникать проблемы, которые будут выражаться в исключениях типа `Smev3Exception`:

```c#
ISmev3Client client = ...;

try
{
   	await client.SendRequestAsync(reqCtx);
}
catch(Smev3Exception e)
{
    var faultCode = e.FaultInfo.faultCode;
    var faultString = e.FaultInfo.faultString;
}
```

## Логирование

Клиент пишет логи о выполненном запросе с указанием дампов запроса и ответа.

Пример:

```
info: MyLab.SmevClient.Smev3Client[0]
      Message: Request sent
      Time: 2023-10-11T15:30:41.229
      Facts:
        request: >+
          POST https://***.com/***/smev
       
          Content-Type: text/xml; charset=utf-8   
          SOAPAction: "urn:Ack" 
          Content-Length: 4623

          <?xml version="1.0" encoding="utf-8"?><s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"><s:Header><Action mustUnderstand="1" xmlns="http://schemas.microsoft.com/ws/2005/05/addressing/none">urn:Ack</Action></s:Header><s:Body><AckRequest xmlns="urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1"><AckTargetMessage Id="SIGNED_BY_CALLER" accepted="true" xmlns="urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.1">ce757035-62a6-11ee-b9a9-02420dfd07d1</AckTargetMessage><CallerInformationSystemSignature><Signature xmlns="http://www.w3.org/2000/09/xmldsig#"><SignedInfo><CanonicalizationMethod Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#" /><SignatureMethod Algorithm="urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-256" /><Reference URI="#SIGNED_BY_CALLER"><Transforms><Transform Algorithm="http://www.w3.org/2000/09/xmldsig#enveloped-signature" /><Transform Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#" /><Transform Algorithm="urn://smev-gov-ru/xmldsig/transform" /></Transforms><DigestMethod Algorithm="urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-256" /><DigestValue>QYL05NJJGHIKr9l5eSgHEsSyWIkjGlL8dWuKECa99yY=</DigestValue></Reference></SignedInfo><SignatureValue>6v1mLypMaIexVA6IUAeUv7Qlv6Yk4EfyZQgULuIWS+ggHjK1gi1CrjUCNHicL41HeXYLAKDBrZhq9IhcCYTCGw==</SignatureValue><KeyInfo><X509Data><X509Certificate>...</X509Certificate></X509Data></KeyInfo></Signature></CallerInformationSystemSignature></AckRequest></s:Body></s:Envelope>
      
        response: >+
          500 
          
          Server: nginx/1.18.0
          Date: Wed, 11 Oct 2023 12:30:41 GMT 
          Connection: keep-alive
          X-Application-Context: application:test:8080
          Accept: text/xml, text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2
          SOAPAction: ""
          Access-Control-Allow-Origin: *
          Access-Control-Allow-Methods: GET, POST, PUT, OPTIONS   
          Access-Control-Allow-Headers: *   
          Access-Control-Expose-Headers: *    
          Content-Type: text/xml; charset=utf-8      
          Content-Length: 1346
          
          <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/"><SOAP-ENV:Header/><SOAP-ENV:Body><SOAP-ENV:Fault><faultcode>SOAP-ENV:Client</faultcode><faultstring xml:lang="ru">Сообщение 'ce757035-62a6-11ee-b9a9-02420dfd07d1' не найдено среди неподтверждённых.</faultstring><detail><ns3:TargetMessageIsNotFound xmlns:ns10="urn://x-artefacts-smev-gov-ru/services/message-exchange/types/routing/1.3" xmlns:ns11="urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.3" xmlns:ns12="urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.3" xmlns:ns2="urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.1" xmlns:ns3="urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.1" xmlns:ns4="urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1" xmlns:ns5="urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.2" xmlns:ns6="urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.2" xmlns:ns7="urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.2" xmlns:ns8="urn://x-artefacts-smev-gov-ru/services/message-exchange/types/directive/1.3" xmlns:ns9="urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.3"/></detail></SOAP-ENV:Fault></SOAP-ENV:Body></SOAP-ENV:Envelope>
```

