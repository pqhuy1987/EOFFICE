using NHG.Core.ClassData;
using NHG.Core.Constants;
using NHG.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NHG.Core.Functions
{
    public class FunctionXML
    {
        private string _strPath;
        private bool _isFileExist = false;
        Log4Net _logger = new Log4Net(typeof(FunctionXML));
        public string StrPath
        {
            get { return _strPath; }
            set { _strPath = value; }
        }

        public FunctionXML(string path)
        {
            _strPath = path;
            if (File.Exists(_strPath))
                _isFileExist = true;
        }

        /// <summary>
        /// Doc du lieu file config
        /// </summary>
        /// <returns></returns>
        public List<string> ReadXMLCorsConfig()
        {
            List<string> lstOrigin = new List<string>();
            try
            {
                XDocument doc = new XDocument(_strPath);
                IEnumerable<XElement> origins = doc.Descendants("origins");
                _logger.Info(origins.Count());
                lstOrigin = (from n in origins.Elements("origin")
                                          select n.ToString()).ToList();
                _logger.Info(lstOrigin.Count);
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }
            return lstOrigin;
        }

        #region Quan ly cong viec
        
        /// <summary>
        /// Doc file xml const
        /// func.ReadConst("status", "new");
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="nameCST"></param>
        /// <returns></returns>
        public CST_Class ReadConst(string groupID, string nameCST)
        {
            try
            {
                CST_Class result = new CST_Class();
                if (_isFileExist)
                {
                    XElement doc = XElement.Load(_strPath);
                    var Groups = (from n in doc.Elements("group")
                              where (string)n.Attribute("id") == groupID
                              select n);
                    var item = (from n in Groups.Descendants("item")
                                where (string)n.Attribute("name") == nameCST
                                select n);
                    if(item.Count() > 0)
                    {
                        XElement xE = (XElement)item.Single();
                        result = new CST_Class() { id = xE.Attribute("id").Value, name = xE.Value };
                        return result;
                    }
                }
                
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }

        public CST_Class ReadConst(string groupID, int id)
        {
            try
            {
                CST_Class result = new CST_Class();
                if (_isFileExist)
                {
                    XElement doc = XElement.Load(_strPath);
                    var Groups = (from n in doc.Elements("group")
                                  where (string)n.Attribute("id") == groupID
                                  select n);
                    var item = (from n in Groups.Descendants("item")
                                where (string)n.Attribute("id") == id.ToString()
                                select n);
                    if (item.Count() > 0)
                    {
                        XElement xE = (XElement)item.Single();
                        result = new CST_Class() { id = xE.Attribute("id").Value, name = xE.Value };
                        return result;
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }
        #endregion

        #region read menu
        public List<MenuLinks> ReadMenuConfig(List<MenuRole> roles, bool isAdmin, string grouptk)
        {
            try
            {
                List<MenuLinks> lstResult = new List<MenuLinks>();
                if (_isFileExist)
                {
                    XElement doc = XElement.Load(_strPath);
                    var list = doc.Elements();
                    foreach (var item in list)
                    {
                        var lstRole = (from n in roles
                                      where n.machucnang == (item.Attribute("id") == null ? "" : item.Attribute("id").Value)
                                      select n).ToList();
                        if ((lstRole.Count > 0 && lstRole.Count == 1) || isAdmin || grouptk == "1")//if ((lstRole.Count > 0 && lstRole.Count == 1) || isAdmin)
                        {
                            MenuLinks mnLink = new MenuLinks()
                            {
                                id = item.Attribute("id") == null ? "" : item.Attribute("id").Value,
                                title = item.Attribute("title") == null ? "" : item.Attribute("title").Value,
                                controller = item.Attribute("controller") == null ? "" : item.Attribute("controller").Value,
                                action = item.Attribute("action") == null ? "" : item.Attribute("action").Value,
                                ImageUrl = item.Attribute("ImageUrl") == null ? "" : item.Attribute("ImageUrl").Value,
                                url = item.Attribute("url") == null ? "" : item.Attribute("url").Value
                            };
                            if (item.Elements().Count() > 0)
                                Iterate(item.Elements(), mnLink, isAdmin, roles, grouptk);
                            lstResult.Add(mnLink);
                        }
                        else if (grouptk == "2" && (item.Attribute("id").Value.ToString() == "50" || item.Attribute("id").Value.ToString() == "70")) //truong phòng xem
                        {
                            string aaa = item.Attribute("id").Value.ToString();
                            MenuLinks mnLink = new MenuLinks()
                            {
                                id = item.Attribute("id") == null ? "" : item.Attribute("id").Value,
                                title = item.Attribute("title") == null ? "" : item.Attribute("title").Value,
                                controller = item.Attribute("controller") == null ? "" : item.Attribute("controller").Value,
                                action = item.Attribute("action") == null ? "" : item.Attribute("action").Value,
                                ImageUrl = item.Attribute("ImageUrl") == null ? "" : item.Attribute("ImageUrl").Value,
                                url = item.Attribute("url") == null ? "" : item.Attribute("url").Value
                            };
                            if (item.Elements().Count() > 0)
                                Iterate(item.Elements(), mnLink, isAdmin, roles, grouptk);
                            lstResult.Add(mnLink);
                        }
                        else if (grouptk == "3" && (item.Attribute("id").Value.ToString() == "50" || item.Attribute("id").Value.ToString() == "70")) // thu ký xem
                        {
                            string aaa = item.Attribute("id").Value.ToString();
                            MenuLinks mnLink = new MenuLinks()
                            {
                                id = item.Attribute("id") == null ? "" : item.Attribute("id").Value,
                                title = item.Attribute("title") == null ? "" : item.Attribute("title").Value,
                                controller = item.Attribute("controller") == null ? "" : item.Attribute("controller").Value,
                                action = item.Attribute("action") == null ? "" : item.Attribute("action").Value,
                                ImageUrl = item.Attribute("ImageUrl") == null ? "" : item.Attribute("ImageUrl").Value,
                                url = item.Attribute("url") == null ? "" : item.Attribute("url").Value
                            };
                            if (item.Elements().Count() > 0)
                                Iterate(item.Elements(), mnLink, isAdmin, roles, grouptk);
                            lstResult.Add(mnLink);
                        }
                        else if (item.Attribute("id").Value.ToString() == "50" || item.Attribute("id").Value.ToString() == "70")
                        {
                            string aaa = item.Attribute("id").Value.ToString();
                            MenuLinks mnLink = new MenuLinks()
                            {
                                id = item.Attribute("id") == null ? "" : item.Attribute("id").Value,
                                title = item.Attribute("title") == null ? "" : item.Attribute("title").Value,
                                controller = item.Attribute("controller") == null ? "" : item.Attribute("controller").Value,
                                action = item.Attribute("action") == null ? "" : item.Attribute("action").Value,
                                ImageUrl = item.Attribute("ImageUrl") == null ? "" : item.Attribute("ImageUrl").Value,
                                url = item.Attribute("url") == null ? "" : item.Attribute("url").Value
                            };
                            if (item.Elements().Count() > 0)
                                Iterate(item.Elements(), mnLink, isAdmin, roles, grouptk);
                            lstResult.Add(mnLink);
                        }

                    }
                }
                return lstResult;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }

        public void Iterate(IEnumerable<XElement> element, MenuLinks menuSub, bool isAdmin, List<MenuRole> roles,string grouptk)
        {
            menuSub.listChild = new List<MenuLinks>();
            foreach (var item in element)
            {
                // kiem tra phan quyen user
                var lstRole = (from n in roles
                               where n.machucnang == (item.Attribute("id") == null ? "" : item.Attribute("id").Value)
                               select n).ToList();
                if ((lstRole.Count > 0 && lstRole.Count == 1) || isAdmin || grouptk=="1")
                {
                    MenuLinks mnLink = new MenuLinks()
                    {
                        id = item.Attribute("id") == null ? "" : item.Attribute("id").Value,
                        title = item.Attribute("title") == null ? "" : item.Attribute("title").Value,
                        controller = item.Attribute("controller") == null ? "" : item.Attribute("controller").Value,
                        action = item.Attribute("action") == null ? "" : item.Attribute("action").Value,
                        ImageUrl = item.Attribute("ImageUrl") == null ? "" : item.Attribute("ImageUrl").Value,
                        url = item.Attribute("url") == null ? "" : item.Attribute("url").Value
                    };
                    if (item.Elements().Count() > 0)
                    {
                        Iterate(item.Elements(), mnLink, isAdmin, roles,grouptk);
                    }
                    menuSub.listChild.Add(mnLink);
                }
                else if (grouptk == "2" && (item.Attribute("id").Value.ToString() == "50" || item.Attribute("id").Value.ToString() == "51" || item.Attribute("id").Value.ToString() == "52"
                    || item.Attribute("id").Value.ToString() == "70" || item.Attribute("id").Value.ToString() == "71"))
                {
                    MenuLinks mnLink = new MenuLinks()
                    {
                        id = item.Attribute("id") == null ? "" : item.Attribute("id").Value,
                        title = item.Attribute("title") == null ? "" : item.Attribute("title").Value,
                        controller = item.Attribute("controller") == null ? "" : item.Attribute("controller").Value,
                        action = item.Attribute("action") == null ? "" : item.Attribute("action").Value,
                        ImageUrl = item.Attribute("ImageUrl") == null ? "" : item.Attribute("ImageUrl").Value,
                        url = item.Attribute("url") == null ? "" : item.Attribute("url").Value
                    };
                    if (item.Elements().Count() > 0)
                    {
                        Iterate(item.Elements(), mnLink, isAdmin, roles, grouptk);
                    }
                    menuSub.listChild.Add(mnLink);
                }

                else if (grouptk == "3" && (item.Attribute("id").Value.ToString() == "50" || item.Attribute("id").Value.ToString() == "51" || item.Attribute("id").Value.ToString() == "52"
                || item.Attribute("id").Value.ToString() == "70" || item.Attribute("id").Value.ToString() == "71"))
                {
                    MenuLinks mnLink = new MenuLinks()
                    {
                        id = item.Attribute("id") == null ? "" : item.Attribute("id").Value,
                        title = item.Attribute("title") == null ? "" : item.Attribute("title").Value,
                        controller = item.Attribute("controller") == null ? "" : item.Attribute("controller").Value,
                        action = item.Attribute("action") == null ? "" : item.Attribute("action").Value,
                        ImageUrl = item.Attribute("ImageUrl") == null ? "" : item.Attribute("ImageUrl").Value,
                        url = item.Attribute("url") == null ? "" : item.Attribute("url").Value
                    };
                    if (item.Elements().Count() > 0)
                    {
                        Iterate(item.Elements(), mnLink, isAdmin, roles, grouptk);
                    }
                    menuSub.listChild.Add(mnLink);
                }


                else if (item.Attribute("id").Value.ToString() == "50" || item.Attribute("id").Value.ToString() == "51" || item.Attribute("id").Value.ToString() == "52"
                    || item.Attribute("id").Value.ToString() == "70" || item.Attribute("id").Value.ToString() == "71")
                {
                    MenuLinks mnLink = new MenuLinks()
                    {
                        id = item.Attribute("id") == null ? "" : item.Attribute("id").Value,
                        title = item.Attribute("title") == null ? "" : item.Attribute("title").Value,
                        controller = item.Attribute("controller") == null ? "" : item.Attribute("controller").Value,
                        action = item.Attribute("action") == null ? "" : item.Attribute("action").Value,
                        ImageUrl = item.Attribute("ImageUrl") == null ? "" : item.Attribute("ImageUrl").Value,
                        url = item.Attribute("url") == null ? "" : item.Attribute("url").Value
                    };
                    if (item.Elements().Count() > 0)
                    {
                        Iterate(item.Elements(), mnLink, isAdmin, roles, grouptk);
                    }
                    menuSub.listChild.Add(mnLink);
                }
            }
        }
        #endregion

        #region
        public string GetStringFormatIDForDataBase(string idTable) {
            try
            {
                List<MenuLinks> lstResult = new List<MenuLinks>();
                if (_isFileExist)
                {
                    XElement doc = XElement.Load(_strPath);
                    var list = doc.Elements();
                    var strFormat = (from n in list
                                   where n.Attribute("id").Value == idTable
                                   select n.Value).Single();
                    if (!string.IsNullOrWhiteSpace(strFormat))
                        return strFormat;
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
            }
            return string.Empty;
        }
        #endregion

        #region get key encrypt xml
        public string ReadXMLGetKeyEncrypt()
        {
            string result = string.Empty;
            try
            {
                XElement doc = XElement.Load(_strPath);
                result = doc.Value;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return result;
        }
        #endregion
    }
}
