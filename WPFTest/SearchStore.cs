using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;


namespace WPFTest
{
    static class SearchStore
    {
        static public List<GameList> gmze = new List<GameList>();
        static private CookieContainer cooks = new CookieContainer();
        static public void TotalSearh(string GameName)
        {
            gmze.Clear();

            gmze.AddRange(SteamSearch(GameName));
            //gmze.AddRange(OriginSearch(GameName));

        }

        static private List<GameList> SteamSearch(string GameName)
        {
            List<GameList> temp = new List<GameList>();
            GameName = GameName.Trim();
            WebRequest webRequ = WebRequest.Create("http://store.steampowered.com/search/suggest?term="+ GameName +"&f=games&cc=RUS&lang=russian&v=2286217");
            HttpWebResponse webRespon = (HttpWebResponse)webRequ.GetResponse();

            try
            {
                Stream strGet = webRespon.GetResponseStream();
                StreamReader reader = new StreamReader(strGet);

                string readthet = reader.ReadToEnd();

                do
                {
                    if (readthet.Contains("match ds_collapse_flag"))
                    {
                        GameList gtmpl = new GameList();
                        int strtIndx = 0;
                        int lstIndx = 0;

                        strtIndx = readthet.IndexOf("match ds_collapse_flag", 0, StringComparison.OrdinalIgnoreCase);
                        string gamecls = readthet.Substring(strtIndx, readthet.IndexOf("</a>") - strtIndx);
                        readthet = readthet.Remove(0, readthet.IndexOf("</a>") + 4);
                        strtIndx = gamecls.IndexOf("href=", StringComparison.OrdinalIgnoreCase);
                        lstIndx = gamecls.IndexOf("\"><");
                        string strRef = gamecls.Substring(strtIndx, lstIndx - strtIndx).Remove(0, 6);
                        gamecls = gamecls.Remove(0, lstIndx);

                        strtIndx = gamecls.IndexOf("match_name", StringComparison.OrdinalIgnoreCase);
                        lstIndx = gamecls.IndexOf("</div");
                        string gname = gamecls.Substring(strtIndx, lstIndx - strtIndx).Remove(0, 12);
                        gamecls = gamecls.Remove(0, lstIndx);

                        strtIndx = gamecls.IndexOf("match_img", StringComparison.OrdinalIgnoreCase);
                        lstIndx = gamecls.IndexOf("\"></div");
                        string gImg = gamecls.Substring(strtIndx, lstIndx - strtIndx);
                        gImg = gImg.Substring(gImg.IndexOf("src=")).Remove(0, 5);
                        gImg = gImg.Trim('"');
                        gamecls = gamecls.Remove(0, lstIndx);

                        strtIndx = gamecls.IndexOf("match_price", StringComparison.OrdinalIgnoreCase);
                        string price = gamecls.Substring(strtIndx);
                        price = price.Remove(0, 13);
                        price = price.Remove(price.IndexOf("</div"));
                        double cost;
                        bool rubs = false;
                        if (price == "Free" || price == "Бесплатно" || price == "Demo" || price == "Демо" || price == "" || price == "Free Demo")
                            cost = 0;
                        else
                        {
                            foreach (char ch in price)
                            {
                                if (!Char.IsDigit(ch) && ch == 'p')
                                {
                                    rubs = true;
                                    break;
                                }
                            }
                            string trump = "";
                            foreach (char ch in price)
                            {
                                if (Char.IsDigit(ch))
                                    trump += ch;
                                if (ch == ',')
                                    trump += ch;
                            }
                            cost = Convert.ToDouble(trump);
                        }

                        gtmpl.refToStore = strRef;
                        gtmpl.GameName = gname;
                        gtmpl.JpgPath = gImg;
                        gtmpl.vaCost = cost;
                        gtmpl.rub = rubs;
                        gtmpl.storeCho = GameList.store.steam;

                        temp.Add(gtmpl);
                    }
                } while (readthet.Contains("match ds_collapse_flag"));

            }
            finally
            { }
                return temp;           
        }

        /*
        static private List<GameList> OriginSearch (string GameName)
        {
            List<GameList> temp = new List<GameList>();
            GameName = GameName.Trim();
            string tempName = "";
            foreach(char ch in GameName)
            {
                if (ch == ' ')
                    tempName += "%2520";
                else if (ch == '&')
                    tempName += "%26";
                else
                    tempName += ch;
            }

            //WebClient webclin = new WebClient();
            //Stream stram = webclin.OpenRead("https://www.origin.com/rus/ru-ru/search?searchString="+tempName);

            //StreamReader readder = new StreamReader(stram);

            string webpge = testingThis("https://www.origin.com/rus/ru-ru/search?searchString="+tempName);
            //webclin.DownloadFile("https://www.origin.com/rus/ru-ru/search?searchString=" + tempName, "UrlTest.txt");

            if (webpge.Contains("noResultsFound"))
                return temp;
            else
            {
                do
                {
                    if (webpge.Contains("origin-search-storelist-item"))
                    {
                        int strtIndx = webpge.IndexOf("origin-search-storelist-item");
                        int endIndx = webpge.IndexOf("</li>", strtIndx);
                        string gamefind = webpge.Substring(strtIndx, endIndx - strtIndx);
                        webpge.Remove(strtIndx, endIndx - strtIndx);

                        strtIndx = gamefind.IndexOf("model.displayName");
                        endIndx = gamefind.IndexOf("</span>", strtIndx);
                        string gmNm = gamefind.Substring(strtIndx+19,endIndx-strtIndx);
                        gamefind = gamefind.Remove(strtIndx, endIndx - strtIndx);
                    }
                } while (webpge.Contains("origin-search-storelist-item"));
            }

            return temp;
        }*/
        /*
        static private string testingThis(string url)
        {
            HttpWebRequest httReq = (HttpWebRequest)WebRequest.Create(url);
            //httReq.Method = "GET";
            httReq.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.90 Safari/537.36";
            httReq.CookieContainer = cooks;
            //httReq.Headers.Add(;
            httReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*//*;q=0.8";
            httReq.Headers.Add("DNT", "1");
            HttpWebResponse resp = (HttpWebResponse)httReq.GetResponse();
            Stream str = resp.GetResponseStream();
            StreamReader stram = new StreamReader(str);
            //resp.Close();
            string httText = stram.ReadToEnd();
            stram.Close();
            resp.Close();

            return httText;
        }
        */
    }
}
