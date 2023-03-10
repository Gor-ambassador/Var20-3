using Hash.DBStructure;
using System.Text;

namespace Hash
{
    internal class Program
    {
        /// <summary>Bad hash function -  43283 (14%) collisions on 300000 elements</summary>
        /// <param name="input">Input string</param>
        /// <returns>UInt32 hash value</returns>
        public static UInt32 BadHash(string input)
        {
            UInt32 result = 0;

            for(UInt32 i = 0; i < input.Length; i += 2)
                result += (Convert.ToUInt32(input[Convert.ToInt32(i)]) << Convert.ToInt32(i % 12)) ^ (Convert.ToUInt32(input[Convert.ToInt32(i)]));
            
            for (UInt32 i = 1; i < input.Length; i += 2)
                result += (Convert.ToUInt32(input[Convert.ToInt32(i)]) >> Convert.ToInt32(i % 12)) ^ (Convert.ToUInt32(input[Convert.ToInt32(i)]));

            return result;
        }

        /// <summary>Good hash function - 67 (0.0002%) collisions on 300000 elements</summary>
        /// <param name="input">Input string</param>
        /// <returns>UInt32 hash value</returns>
        public static UInt32 GoodHash(string input)
        {
            UInt32 result = 0;

            for (UInt32 i = 0; i < input.Length; i += 2)
                result += (Convert.ToUInt32(input[Convert.ToInt32(i)]) << Convert.ToInt32(i % 23)) ^ (Convert.ToUInt32(input[Convert.ToInt32(i)]));

            for (UInt32 i = 1; i < input.Length; i += 2)
                result += (Convert.ToUInt32(input[Convert.ToInt32(i)]) >> Convert.ToInt32(i % 23)) ^ (Convert.ToUInt32(input[Convert.ToInt32(i)]));

            return result;
        }

        /// <summary>Function that searches the element by GoodHash in LookUp</summary>
        /// <param name="data">Input LookUp</param>
        /// <param name="find">Element to find</param>
        /// <returns>Element found</returns>
        static InfoGood SearchElement(Lookup<UInt32, InfoGood> data, InfoGood find)
        {
            return data.Where(x => x.Key == find.Hash).SelectMany(x => x).First(x => x == find);
        }

        /// <summary>Function that searches the element by BadHash in LookUp</summary>
        /// <param name="data">Input LookUp</param>
        /// <param name="find">Element to find</param>
        /// <returns>Element found</returns>
        static InfoBad SearchElement(Lookup<UInt32, InfoBad> data, InfoBad find)
        {
            return data.Where(x => x.Key == find.Hash).SelectMany(x => x).First(x => x == find);
        }

        static void Main(string[] args)
        {
            Random random = new Random();

           using (var db = new DataContext100())
            {
                List<BaseInfo> infos = db.Info.ToList();
                List<InfoGood> infosGood = infos.Select(x => new InfoGood(x)).ToList();
                List<InfoBad> infosBad = infos.Select(x => new InfoBad(x)).ToList();

                Lookup<UInt32, InfoGood> infosGoodLook = (Lookup<UInt32, InfoGood>)infosGood.ToLookup(x => x.Hash, x => x);
                Lookup<UInt32, InfoBad> infosBadLook = (Lookup<UInt32, InfoBad>)infosBad.ToLookup(x => x.Hash, x => x);

                InfoGood findGood = infosGood[random.Next(infosGood.Count)];

                DateTime start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                findGood = infosGood[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                DateTime end = DateTime.Now;

                Console.WriteLine($"Good Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element");

                InfoBad findBad = infosBad[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosBadLook, findBad);

                end = DateTime.Now;

                Console.WriteLine($"Bad Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element \n");

                Console.WriteLine($"GoodHash, {infos.Count} elements, {infosGood.Count - infosGoodLook.Count} collisions");
                Console.WriteLine($"BadHash, {infos.Count} elements, {infosBad.Count - infosBadLook.Count} collisions \n\n");
            };
     
            using (var db = new DataContext1000())
            {
                List<BaseInfo> infos = db.Info.ToList();
                List<InfoGood> infosGood = infos.Select(x => new InfoGood(x)).ToList();
                List<InfoBad> infosBad = infos.Select(x => new InfoBad(x)).ToList();

                Lookup<UInt32, InfoGood> infosGoodLook = (Lookup<UInt32, InfoGood>)infosGood.ToLookup(x => x.Hash, x => x);
                Lookup<UInt32, InfoBad> infosBadLook = (Lookup<UInt32, InfoBad>)infosBad.ToLookup(x => x.Hash, x => x);

                InfoGood findGood = infosGood[random.Next(infosGood.Count)];

                DateTime start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                findGood = infosGood[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                DateTime end = DateTime.Now;

                Console.WriteLine($"Good Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element");

                InfoBad findBad = infosBad[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosBadLook, findBad);

                end = DateTime.Now;

                Console.WriteLine($"Bad Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element \n");

                Console.WriteLine($"GoodHash, {infos.Count} elements, {infosGood.Count - infosGoodLook.Count} collisions");
                Console.WriteLine($"BadHash, {infos.Count} elements, {infosBad.Count - infosBadLook.Count} collisions \n \n");
            };

            using (var db = new DataContext10000())
            {
                List<BaseInfo> infos = db.Info.ToList();
                List<InfoGood> infosGood = infos.Select(x => new InfoGood(x)).ToList();
                List<InfoBad> infosBad = infos.Select(x => new InfoBad(x)).ToList();

                Lookup<UInt32, InfoGood> infosGoodLook = (Lookup<UInt32, InfoGood>)infosGood.ToLookup(x => x.Hash, x => x);
                Lookup<UInt32, InfoBad> infosBadLook = (Lookup<UInt32, InfoBad>)infosBad.ToLookup(x => x.Hash, x => x);

                InfoGood findGood = infosGood[random.Next(infosGood.Count)];

                DateTime start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                findGood = infosGood[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                DateTime end = DateTime.Now;

                Console.WriteLine($"Good Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element");

                InfoBad findBad = infosBad[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosBadLook, findBad);

                end = DateTime.Now;

                Console.WriteLine($"Bad Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element \n");

                Console.WriteLine($"GoodHash, {infos.Count} elements, {infosGood.Count - infosGoodLook.Count} collisions");
                Console.WriteLine($"BadHash, {infos.Count} elements, {infosBad.Count - infosBadLook.Count} collisions \n \n");
            };

            using (var db = new DataContext20000())
            {
                List<BaseInfo> infos = db.Info.ToList();
                List<InfoGood> infosGood = infos.Select(x => new InfoGood(x)).ToList();
                List<InfoBad> infosBad = infos.Select(x => new InfoBad(x)).ToList();

                Lookup<UInt32, InfoGood> infosGoodLook = (Lookup<UInt32, InfoGood>)infosGood.ToLookup(x => x.Hash, x => x);
                Lookup<UInt32, InfoBad> infosBadLook = (Lookup<UInt32, InfoBad>)infosBad.ToLookup(x => x.Hash, x => x);

                InfoGood findGood = infosGood[random.Next(infosGood.Count)];

                DateTime start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                findGood = infosGood[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                DateTime end = DateTime.Now;

                Console.WriteLine($"Good Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element");

                InfoBad findBad = infosBad[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosBadLook, findBad);

                end = DateTime.Now;

                Console.WriteLine($"Bad Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element \n");

                Console.WriteLine($"GoodHash, {infos.Count} elements, {infosGood.Count - infosGoodLook.Count} collisions");
                Console.WriteLine($"BadHash, {infos.Count} elements, {infosBad.Count - infosBadLook.Count} collisions \n \n");
            };

            using (var db = new DataContext40000())
            {
                List<BaseInfo> infos = db.Info.ToList();
                List<InfoGood> infosGood = infos.Select(x => new InfoGood(x)).ToList();
                List<InfoBad> infosBad = infos.Select(x => new InfoBad(x)).ToList();

                Lookup<UInt32, InfoGood> infosGoodLook = (Lookup<UInt32, InfoGood>)infosGood.ToLookup(x => x.Hash, x => x);
                Lookup<UInt32, InfoBad> infosBadLook = (Lookup<UInt32, InfoBad>)infosBad.ToLookup(x => x.Hash, x => x);

                InfoGood findGood = infosGood[random.Next(infosGood.Count)];

                DateTime start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                findGood = infosGood[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                DateTime end = DateTime.Now;

                Console.WriteLine($"Good Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element");

                InfoBad findBad = infosBad[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosBadLook, findBad);

                end = DateTime.Now;

                Console.WriteLine($"Bad Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element \n");

                Console.WriteLine($"GoodHash, {infos.Count} elements, {infosGood.Count - infosGoodLook.Count} collisions");
                Console.WriteLine($"BadHash, {infos.Count} elements, {infosBad.Count - infosBadLook.Count} collisions \n \n");
            };

            using (var db = new DataContext60000())
            {
                List<BaseInfo> infos = db.Info.ToList();
                List<InfoGood> infosGood = infos.Select(x => new InfoGood(x)).ToList();
                List<InfoBad> infosBad = infos.Select(x => new InfoBad(x)).ToList();

                Lookup<UInt32, InfoGood> infosGoodLook = (Lookup<UInt32, InfoGood>)infosGood.ToLookup(x => x.Hash, x => x);
                Lookup<UInt32, InfoBad> infosBadLook = (Lookup<UInt32, InfoBad>)infosBad.ToLookup(x => x.Hash, x => x);

                InfoGood findGood = infosGood[random.Next(infosGood.Count)];

                DateTime start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                findGood = infosGood[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                DateTime end = DateTime.Now;

                Console.WriteLine($"Good Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element");

                InfoBad findBad = infosBad[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosBadLook, findBad);

                end = DateTime.Now;

                Console.WriteLine($"Bad Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element \n");

                Console.WriteLine($"GoodHash, {infos.Count} elements, {infosGood.Count - infosGoodLook.Count} collisions");
                Console.WriteLine($"BadHash, {infos.Count} elements, {infosBad.Count - infosBadLook.Count} collisions \n \n");
            };

            using (var db = new DataContext80000())
            {
                List<BaseInfo> infos = db.Info.ToList();
                List<InfoGood> infosGood = infos.Select(x => new InfoGood(x)).ToList();
                List<InfoBad> infosBad = infos.Select(x => new InfoBad(x)).ToList();

                Lookup<UInt32, InfoGood> infosGoodLook = (Lookup<UInt32, InfoGood>)infosGood.ToLookup(x => x.Hash, x => x);
                Lookup<UInt32, InfoBad> infosBadLook = (Lookup<UInt32, InfoBad>)infosBad.ToLookup(x => x.Hash, x => x);

                InfoGood findGood = infosGood[random.Next(infosGood.Count)];

                DateTime start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                findGood = infosGood[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                DateTime end = DateTime.Now;

                Console.WriteLine($"Good Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element");

                InfoBad findBad = infosBad[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosBadLook, findBad);

                end = DateTime.Now;

                Console.WriteLine($"Bad Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element \n");

                Console.WriteLine($"GoodHash, {infos.Count} elements, {infosGood.Count - infosGoodLook.Count} collisions");
                Console.WriteLine($"BadHash, {infos.Count} elements, {infosBad.Count - infosBadLook.Count} collisions \n \n");
            };

            using (var db = new DataContext100000())
            {
                List<BaseInfo> infos = db.Info.ToList();
                List<InfoGood> infosGood = infos.Select(x => new InfoGood(x)).ToList();
                List<InfoBad> infosBad = infos.Select(x => new InfoBad(x)).ToList();

                Lookup<UInt32, InfoGood> infosGoodLook = (Lookup<UInt32, InfoGood>)infosGood.ToLookup(x => x.Hash, x => x);
                Lookup<UInt32, InfoBad> infosBadLook = (Lookup<UInt32, InfoBad>)infosBad.ToLookup(x => x.Hash, x => x);

                InfoGood findGood = infosGood[random.Next(infosGood.Count)];

                DateTime start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                findGood = infosGood[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                DateTime end = DateTime.Now;

                Console.WriteLine($"Good Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element");

                InfoBad findBad = infosBad[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosBadLook, findBad);

                end = DateTime.Now;

                Console.WriteLine($"Bad Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element \n");

                Console.WriteLine($"GoodHash, {infos.Count} elements, {infosGood.Count - infosGoodLook.Count} collisions");
                Console.WriteLine($"BadHash, {infos.Count} elements, {infosBad.Count - infosBadLook.Count} collisions \n \n");
            };

            using (var db = new DataContext300000())
            {
                List<BaseInfo> infos = db.Info.ToList();
                List<InfoGood> infosGood = infos.Select(x => new InfoGood(x)).ToList();
                List<InfoBad> infosBad = infos.Select(x => new InfoBad(x)).ToList();

                Lookup<UInt32, InfoGood> infosGoodLook = (Lookup<UInt32, InfoGood>)infosGood.ToLookup(x => x.Hash, x => x);
                Lookup<UInt32, InfoBad> infosBadLook = (Lookup<UInt32, InfoBad>)infosBad.ToLookup(x => x.Hash, x => x);

                InfoGood findGood = infosGood[random.Next(infosGood.Count)];

                DateTime start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                findGood = infosGood[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosGoodLook, findGood);

                DateTime end = DateTime.Now;

                Console.WriteLine($"Good Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element");

                InfoBad findBad = infosBad[random.Next(infosGood.Count)];

                start = DateTime.Now;

                SearchElement(infosBadLook, findBad);

                end = DateTime.Now;

                Console.WriteLine($"Bad Hash, {infos.Count} elements, {(end - start).TotalMilliseconds} time to find element \n");

                Console.WriteLine($"GoodHash, {infos.Count} elements, {infosGood.Count - infosGoodLook.Count} collisions");
                Console.WriteLine($"BadHash, {infos.Count} elements, {infosBad.Count - infosBadLook.Count} collisions \n \n");
            };
        }
    }
}