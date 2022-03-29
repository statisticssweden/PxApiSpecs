﻿using PxApi2Dummy.Data;

namespace PxApi2Dummy.SampleData
{
    public class SampleNavigate
    {
        private Dictionary<string, MenuItem> data;
        private Dictionary<string, Api1Client.ApiV1MenuItem[]> apiV1Data;

        private readonly string urlNavTreeFormat;
        private const string tableHrefFormat = "https://my-site.com/api/v2/tables/{0}";

        public static SampleNavigate GetSampleNavigate(string urlNavTreeFormat)
        {
            if (_instance == null)
            {
                _instance = new SampleNavigate(urlNavTreeFormat+ "/{0}");
            }
            return _instance;
        }

        private static SampleNavigate _instance;

        private SampleNavigate(string urlNavTreeFormat)
        {
            this.urlNavTreeFormat = urlNavTreeFormat;

            int sortOrder = 1;


            apiV1Data = GetOldDataWithNewKeyAndID();

            data = new Dictionary<string, MenuItem>();


            MenuItem root = new MenuItem() { id = "", menuItemType = MenuItemType.ROOT };

            root.children = GetKids(apiV1Data[""]);

            data.Add("", root);


            foreach (MenuItem item in root.children)
            {
                CrawlChildren(item);
            }
            
        }


        private MenuItem[] GetKids(Api1Client.ApiV1MenuItem[] rootOld)
        {
            List<MenuItem> kids = new List<MenuItem>();
            foreach (var item in rootOld)
            {

                MenuItem temp = new MenuItem() { id = item.id, label = item.text };

                if (item.type == "l")
                {
                    temp.menuItemType = MenuItemType.FOLDER_CLOSED;
                    temp.links = new Link[]
                         { new Link() { rel = "data", href = String.Format(urlNavTreeFormat , temp.id) } };
                }
                else if (item.type == "h")
                {
                    temp.menuItemType = MenuItemType.HEADING;
                }
                else if (item.type == "t")
                {
                    temp.menuItemType = MenuItemType.TABLE;
                    temp.links = new Link[]
                         { new Link() { rel = "metadata", href = String.Format(tableHrefFormat, temp.id) } };
                }
                else
                {
                    string a = "sdf";
                }

                kids.Add(temp);
            }
            var myOut = kids.ToArray();
            return myOut;
        }

        private Dictionary<string, Api1Client.ApiV1MenuItem[]> GetOldDataWithNewKeyAndID()
        {
            Api1Client.ApiV1Client tmp = new Api1Client.ApiV1Client();
            Dictionary<string, Api1Client.ApiV1MenuItem[]> myOut = new Dictionary<string, Api1Client.ApiV1MenuItem[]>();

            var old = tmp.Properties;

            foreach (var oldKey in old.Keys)
            {
                string newKey = "";
                Api1Client.ApiV1MenuItem[] theValue = old[oldKey];

                if (!(oldKey == ""))
                {
                    newKey = oldKey.Substring(1).Replace("/", "_");

                    foreach (Api1Client.ApiV1MenuItem oldItem in theValue)
                    {
                        if (oldItem.type == "l")
                        {
                            oldItem.id = newKey + "_" + oldItem.id;
                        }
                    }
                }
                myOut.Add(newKey, theValue);
            }

            return myOut;
        }

        private void CrawlChildren(MenuItem aItemWithOutChildren)
        {
            // if inline  MenuItem itemWithOutChildren =  aItemWithOutChildren
            // 
            MenuItem itemWithOutChildren = new MenuItem() { id = aItemWithOutChildren.id, label = aItemWithOutChildren.label, menuItemType = aItemWithOutChildren.menuItemType };


            if (!(apiV1Data.ContainsKey(itemWithOutChildren.id)))
            {
                string hmm = "hhm";
            }

            itemWithOutChildren.children = GetKids(apiV1Data[itemWithOutChildren.id]);
            data.Add(itemWithOutChildren.id, itemWithOutChildren);

            foreach (MenuItem item in itemWithOutChildren.children)
            {
                if (item.menuItemType.Equals(MenuItemType.HEADING) || item.menuItemType.Equals(MenuItemType.TABLE))
                {
                    continue;
                }
                CrawlChildren(item);
            }
        }

        public MenuItem Get(string id)
        {
            return data[id];
        }

    }
}