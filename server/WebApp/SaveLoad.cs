using Microsoft.Extensions.Options;
using PackagingGenetic;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace WebApp
{

    public static class SaveLoad
    {
        public static string DirPath = "C:\\Users\\amoravon\\Documents\\sharp2024\\server\\storage\\";
        public static string MainFile = "Experiments.json";
        public static string FileEnding = "_Data.json";
        

        public static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        public static void Save(Experiment Ex)
        {
            List<Experiment>? ExList = null;
            string FileContent = File.ReadAllText(DirPath + MainFile);
            if (FileContent != "")
            {
                ExList = JsonSerializer.Deserialize<List<Experiment>>(FileContent);
            }
            if (ExList == null)
            {
                ExList = new();
            }
            ExList.Add(Ex);
            File.WriteAllText(DirPath + MainFile, JsonSerializer.Serialize(ExList, options));
            File.WriteAllText(DirPath + Ex.ExName + FileEnding, JsonSerializer.Serialize(Ex.ExPopulation, options));
        }
        public static Experiment? Load(string Name)
        {
            List<Experiment>? ExList = null;
            string FileContent = File.ReadAllText(DirPath + MainFile);
            if (FileContent != "")
            {
                ExList = JsonSerializer.Deserialize<List<Experiment>>(FileContent);
            }
            if (ExList == null)
            {
                return null;
            }
            foreach (var Ex in ExList)
            {
                if (Ex.ExName == Name)
                {
                    string ExFileContent = File.ReadAllText(DirPath + Name + FileEnding);
                    Ex.ExPopulation = JsonSerializer.Deserialize<Population>(ExFileContent);
                    return Ex;
                }
            }
            return null;
        }

        public static int Update(Experiment NewEx)
        {
            List<Experiment>? ExList = JsonSerializer.Deserialize<List<Experiment>>(File.ReadAllText(DirPath + MainFile));
            if (ExList == null)
            {
                return -1;
            }
            Experiment? OldEx = null;
            foreach (var Ex in ExList)
            {
                if (Ex.ExName == NewEx.ExName)
                {
                    OldEx = Ex;
                }
            }
            if (OldEx == null)
            {
                return -1;
            }
            ExList.Remove(OldEx);
            ExList.Add(NewEx);
            File.WriteAllText(DirPath + MainFile, JsonSerializer.Serialize(ExList, options));
            File.WriteAllText(DirPath + NewEx.ExName + FileEnding, JsonSerializer.Serialize(NewEx.ExPopulation, options));
            return 0;
        }
        public static int Delete(string Name)
        {
            List<Experiment>? ExList = JsonSerializer.Deserialize<List<Experiment>>(File.ReadAllText(DirPath + MainFile));
            if (ExList == null)
            {
                return -1;
            }
            Experiment? DelEx = null;
            foreach (var Ex in ExList)
            {
                if (Ex.ExName == Name)
                {
                    DelEx = Ex;
                }
            }
            if (DelEx == null)
            {
                return -1;
            }
            ExList.Remove(DelEx);
            File.WriteAllText(DirPath + MainFile, JsonSerializer.Serialize(ExList, options));
            File.Delete(DirPath + DelEx.ExName + FileEnding);
            return 0;
        }


        public static List<string>? GetAllNames()
        {
            string FileContent = File.ReadAllText(DirPath + MainFile);
            if (FileContent == "")
            {
                return null;
            }
            List<Experiment>? ExList = JsonSerializer.Deserialize<List<Experiment>>(FileContent);
            if (ExList == null)
            {
                return null;
            }
            List<string> Names = new();
            foreach (var Ex in ExList)
            {
                Names.Add(Ex.ExName);
            }
            return Names;
        }
    }
}
