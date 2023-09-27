using System.Reflection;

namespace Utility
{
    public class Entity
    {
        public int Id { get; set; }
        public Entity() { }
        public Entity(int id)
        {
            Id = id;
        }
        public override string ToString()
        {
            string ris = "";

            foreach (PropertyInfo proprieta in this.GetType().GetProperties())
                ris += $"{proprieta.Name}: {proprieta.GetValue(this)}\n";

            return ris;
        }
        public void FromDictionary(Dictionary<string, string> item)
        {
            Dictionary<string, string> newitem = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in item)
            {
                newitem.Add(pair.Key.ToLower(), pair.Value);
            }
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                if (item.ContainsKey(property.Name.ToLower()))
                {

                    object valore = item[property.Name.ToLower()];
                    switch (property.PropertyType.Name.ToLower())
                    {
                        case "int32":
                            valore = int.Parse(item[property.Name.ToLower()]);
                            break;
                        case "double":
                            valore = double.Parse(Database.CambiaPunti(item[property.Name.ToLower()]));
                            break;
                        case "boolean":
                            valore = bool.Parse(item[property.Name.ToLower()]);
                            break;
                        case "datetime":
                            try
                            {
                                valore = DateTime.Parse(item[property.Name.ToLower()]);
                            }
                            catch
                            {
                                valore = null;
                            }
                            break;
                    }
                    property.SetValue(this, valore);
                }
            }
        }
    }
}
