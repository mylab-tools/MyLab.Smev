namespace MyLab.SmevClient
{
    public interface ISmev3ClientFactory
    {
        /// <summary>
        /// Создание клиента по мнемонике сервиса
        /// </summary>
        /// <param name="mnemonic">Мнемоника сервиса в СМЭВ</param>
        /// <returns></returns>
        ISmev3Client Get(string mnemonic);
    }
}
