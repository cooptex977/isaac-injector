﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace OML
{
    internal class Loader
    {
        public static bool FirstTimeFlag = false;

        public static bool init(ref string path)
        {
            if (!Directory.Exists("Plugins"))
                Directory.CreateDirectory("Plugins");
            if (!Directory.Exists("res"))
                Directory.CreateDirectory("res");
            if (!Directory.Exists("res\\dll"))
                Directory.CreateDirectory("dll");
            if (!Directory.Exists("res\\xml"))
                Directory.CreateDirectory("res\\xml");
            if (!File.Exists(".usersettings"))
                File.Create(".usersettings").Close();
            string s;
            using (StreamReader reader = new StreamReader(".usersettings"))
            {
                s = reader.ReadLine();
            }
            if (s == null)
            {
                using (StreamWriter writer = new StreamWriter(".usersettings", true))
                {
                    writer.WriteLine("false");
                }
                FirstTimeFlag = true;
            }
            else if (s == "false")
            {
                FirstTimeFlag = false;
                using (StreamReader reader = new StreamReader(".usersettings"))
                {
                    reader.ReadLine();
                    path = reader.ReadLine();
                }
            }
            return true;
        }
        public static List<OMLPlugin> GetPlugins()
        {
            string[] dlls = Directory.GetFiles("Plugins", "*.dll").Where(x => x != "Plugins\\OML.dll").ToArray<string>();
            List<Assembly> assemblies = new List<Assembly>();
            foreach (string dll in dlls)
                assemblies.Add(Assembly.LoadFile(Path.GetFullPath(dll)));
            List<OMLPlugin> plugins = new List<OMLPlugin>();
            foreach (Assembly a in assemblies)
                if (a != null)
                    foreach (Type t in a.GetTypes())
                        if (t.IsSubclassOf(typeof(OMLPlugin)))
                            plugins.Add((OMLPlugin)Activator.CreateInstance(t));
            return plugins;
        }
        public static List<Item> LoadItems(List<OMLPlugin> plugins, string path)
        {
            string respath = path + "\\resources";
            if (!Directory.Exists(respath))
                Directory.CreateDirectory(respath);
            File.Copy("res\\xml\\items.xml", respath + "\\items.xml", true);
            File.Copy("res\\xml\\itempools.xml", respath + "\\itempools.xml", true);
            List<Item> ret = new List<Item>();
            int id = -1;
            foreach (OMLPlugin plugin in plugins)
            {
                foreach (Item i in plugin.CustomItemList)
                {
                    i.id = id;
                    ret.Add(i);
                    switch(i.resource.resourceType)
                    {
                        case ResourceType.Item:
                            File.Copy("Plugins\\PluginResources\\" + i.resource.Path, respath + "\\gfx\\items\\collectibles\\" + i.resource.resourceName, true);
                            break;
                    }
                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.OmitXmlDeclaration = true;

                    XElement node;


                    if (!String.IsNullOrEmpty(i.cache))
                    {
                        node = new XElement(i.Type.ToString(),
                           new XAttribute("cache", i.cache),
                           new XAttribute("description", i.PickupText),
                           new XAttribute("gfx", i.resource.resourceName),
                           new XAttribute("id", i.id),
                           new XAttribute("name", i.Name));
                    }
                    else
                    {
                        node = new XElement(i.Type.ToString(),
                               new XAttribute("description", i.PickupText),
                               new XAttribute("gfx", i.resource.resourceName),
                               new XAttribute("id", i.id),
                               new XAttribute("name", i.Name));
                    }

                    XDocument xdoc = XDocument.Load(respath + "\\items.xml");
                    xdoc.Element("items").Nodes().Last().AddAfterSelf(node);
                    xdoc.Save(respath + "\\items.xml");


                    XElement pnode;
                    pnode = new XElement("Item",
                        new XAttribute("Id", i.id),
                        new XAttribute("Weight", i.Weight),
                        new XAttribute("DecreaseBy", i.DecreaseBy),
                        new XAttribute("RemoveOn", i.RemoveOn));
                    xdoc = XDocument.Load(respath + "\\itempools.xml");
                    xdoc.Element("ItemPools").Element("Pool").Nodes().Last().AddAfterSelf(pnode);
                    //xdoc.Save(respath + "\\itempools.xml");
                    id--;
                }
            }
            return ret;
        }
    }
    public class MyItem : Item { }
}
