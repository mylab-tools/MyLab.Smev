using MyLab.SmevClient.Smev;

namespace MyLab.SmevClient
{
    public interface ISmev3Envelope
    {
        /// <summary>
        /// Получение содержимого конверта
        /// </summary>
        /// <returns></returns>
        byte[] Get();

        /// <summary>
        /// СМЭВ3 метод
        /// </summary>
        Smev3Methods SmevMethod { get; }
    }
}
