using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CreateSelfSignedCertificate
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateCertificate(@"C:\temp\testcert.pfx", "HelloWorld");
        }
        static void CreateCertificate(string path, string password = null)
        {
            var ecdsa = ECDsa.Create();

            var request = new CertificateRequest("CN=testcert, O=GIBZ, L=Zug, S=Zug, C=CH", ecdsa, HashAlgorithmName.SHA512);

            var cert = request.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));

            byte[] exportedCert = null;
            if (password == null)
            {
                exportedCert = cert.Export(X509ContentType.Pfx);
            }
            else
            {
                exportedCert = cert.Export(X509ContentType.Pfx, password);
            }
            if (!Directory.Exists(Path.GetDirectoryName(path))) Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllBytes(path, exportedCert);
        }
    }
}
