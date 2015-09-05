
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace PostWatcher
{
    public interface IComponent
    {
       void LoadFromXml(XmlNode xmlDoc);
    }
}
