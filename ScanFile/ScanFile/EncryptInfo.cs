namespace ScanFile
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class EncryptInfo
    {
        public EncryptInfo(string source)
        {
            this.Source = source;
            this.Encode = Encoding.Default;
        }

        public EncryptInfo(string source, Encoding encode)
        {
            this.Source = source;
            this.Encode = encode;
        }

        public Encoding Encode { get; set; }

        public string Iv { get; set; }

        public string Key { get; set; }

        public string Source { get; set; }
    }
}

