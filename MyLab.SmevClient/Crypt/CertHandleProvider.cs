namespace MyLab.SmevClient.Crypt
{
    abstract public class CertHandleProvider
    {
        internal abstract CertContextSafeHandleHandler Provide();
    }
}