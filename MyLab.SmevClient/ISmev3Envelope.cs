namespace MyLab.SmevClient
{
    public interface ISmev3Envelope
    {
        /// <summary>
        /// Получение содержимого конверта
        /// </summary>
        /// <returns></returns>
        byte[] Get();
    }
}
