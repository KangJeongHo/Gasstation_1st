#region 유니티 내에서 제이슨 저장하고 불러오는 스크립트

/*
[System.Serializable]
public class CarsData
{
    public string mCarName;

    public enum MCarGasType { Gasoline, Diesel, Lpg, Bio, Electronic, ox }

    public int mCarGasAmountMin,
        mCarGasAmountMax,
        mCarGasAmountRand,
        mCarLimitTimeMin,
        mCarLimitTimeMax,
        mCarLimitTimeRand,
        mCarSatisfactionMin,
        mCarSatisfactionMax,
        mCarSatisfactionRand,
        mCarZenProbability,
        mCarOpenLevel;

    public string mCarImage;

    public bool mIsActive;

    public void carsDataList()
    {
        Debug.Log(mCarName);
        
    } 
}

public class CarsArray
{
    public CarsData[] CarsLists;
}


void Start()
{
    //string filePath = "Assets/Resources/Jsons/CarDatas.json";
    //string json = File.ReadAllText(filePath);
    TextAsset carsAsset = Resources.Load<TextAsset>("Jsons/CarDatas");
    Debug.Log(carsAsset);
    CarsData cd = JsonUtility.FromJson<CarsData>(carsAsset.text);
    
    Debug.Log(cd);

    //아래는 값을 수정하고 제이슨 파일을 변경하는데 쓰임
    //cd.mCarName = "RedCar";
    //string classToJson = JsonUtility.ToJson(cd);
    //File.WriteAllText(filePath, classToJson);
}
*/

#endregion

#region 배열 json 파싱 불러오기

/*
    public class JsonHelper
    {
        public static T[] FromJson<T>(string jsonFile)
        {
            //Wrapper 뜻 : 포장지 
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(jsonFile);
            return wrapper.carsList;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.carsList = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.carsList = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] carsList;
        }
    }

    public class CarsData
    {
        public string MCarName;

        public enum MCarGasType { Gasoline, Diesel, Lpg, Bio, Electronic, ox }

        public int MCarGasAmountMin,
            MCarGasAmountMax,
            MCarGasAmountRand,
            MCarLimitTimeMin,
            MCarLimitTimeMax,
            MCarLimitTimeRand,
            MCarSatisfactionMin,
            MCarSatisfactionMax,
            MCarSatisfactionRand,
            MCarZenProbability,
            MCarOpenLevel;

        public string MCarImage;
        public bool MIsActive;
    }

    public CarsData[] carsList;

    void Start()
    {
        string fixJson(string value)
        {
            value = "{\n\"carsList\": " + value + "\n}";
            return value;
        }

        //제이슨 파일을 불러오는 첫번쨰 방법
        //string filePath = "Assets/Resources/Jsons/CarDatas.json";
        //string carsAsset = File.ReadAllText(filePath);
        //제이슨 파일을 불러오는 두번째 방법
        var carsAsset = Resources.Load<TextAsset>("Jsons/carsData");


        string jsonString = fixJson(carsAsset.ToString());
        Debug.Log(jsonString);
        CarsData[] carsList = JsonHelper.FromJson<CarsData>(carsAsset.ToString());
        Debug.Log(carsList);

        //아래는 값을 수정하고 제이슨 파일을 변경하는데 쓰임
        //cd.mCarName = "RedCar";
        //string classToJson = JsonUtility.ToJson(cd);
        //File.WriteAllText(filePath, classToJson);
    }
*/

#endregion
