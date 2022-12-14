using System.ComponentModel;
using System.Xml.Serialization;

namespace ShopData
{
    public class IdName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IdName() => (Id, Name) = (0, string.Empty);
    }

    public class Group : IdName {}
    
    public class Product : IdName 
    { 
        public Group Group { get; set; }
        public decimal Price { get; set; }
    }

    public class Customer : IdName 
    { 
        public string Password { get; set; }    
    }

    public class Order : IdName 
    { 
    }

    public class IdNameList<T> : BindingList<T> where T : IdName
    {
        public int LastId { get; set; } 
        public new void AddNew()
        {
            T item = base.AddNew();
            item.Id = this.Max(it => it.Id) + 1;
        }
    }

    public class Shop
    {
        public IdNameList<Group> Groups { get; set; }
        public IdNameList<Product> Products { get; set; }
        public IdNameList<Customer> Customers { get; set; }

        public Shop()
        {
            Groups = new IdNameList<Group>();
            Products = new IdNameList<Product>();
            Customers = new IdNameList<Customer>();
        }
    }

    public class XmlFile
    {
        string fileName;
        XmlSerializer serializer;

        public XmlFile(string fileName)
        {
            this.fileName = fileName;
            serializer = new XmlSerializer(typeof(Shop));
        }

        public Shop Open()
        {
            try
            {
                using FileStream file = new FileStream(fileName, FileMode.Open);
                return (Shop)serializer.Deserialize(file);
            }
            catch
            {
                return new Shop();
            }
        }

        public void Save(Shop shop)
        {
            using FileStream file = new FileStream(fileName, FileMode.Create);
            serializer.Serialize(file, shop);
        }
    }
}