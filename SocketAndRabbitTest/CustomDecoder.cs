using SuperSocket.ProtoBase;
using System.Buffers;
using System.Text;

namespace Server
{
    public class CustomDecoder : IPackageDecoder<StringPackageInfo>
    {
        private string _text;
        public StringPackageInfo Decode(ref ReadOnlySequence<byte> buffer, object context)
        {

            try
            {
                _text = buffer.GetString(new UTF8Encoding(false));

                var key = _text.Substring(0, 3).ToUpper();
                //var data = _text.Substring(4).ToUpper();
                //var parameters = data.Split('|');
              
                var parameters = _text.ToUpper().Split('|');

                return new StringPackageInfo { Key = key, Body = _text, Parameters = parameters };
            }
            catch (Exception)
            {
                const string key = "ERR";
                return new StringPackageInfo { Key = key, Body = _text, Parameters = null };

            }
        }
    }
}