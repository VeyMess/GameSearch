using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFTest
{
    class GameList
    {
        public string GameName;
        public string JpgPath;
        public double vaCost;
        public string refToStore;
        public bool rub;
        public enum store { steam,gog,origin,psn,xLive};

        public store storeCho;

        public GameList (string name, string path, int loo, double cost, string store)
        {
            GameName = name;
            JpgPath = path;
            storeCho = (store)loo;
            vaCost = cost;
            refToStore = store;
        }
        public GameList() { }
    }
}
