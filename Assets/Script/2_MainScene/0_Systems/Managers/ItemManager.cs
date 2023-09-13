using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LubNames
{
    RedLub, BlueLub, GreenLub, YellowLub, FuckLub
}
[System.Serializable]
public class LubInfo
{
    public LubNames LubNames { get; }
    public int MLubChargingSpeed { get; set; }
    public double MLubPrice { get; set; }
    public string[] MGasType { get; set; }
    public bool MBuyIsActive { get; set; }
    
        //주유기에 들어가야 할 값. 
        //주유기 이름, 주유기 속도, 주유기 가격, 주유 가능 종류(주유기들의 종류를 늘려 여러가지 주유가 가능한 주유기를 사도록 유도할 생각), 기타(경험치, 추가 만족도, 손님 대기시간 증가효과, 인지도, 돈 버프, 주유속도 버프 등)

        public LubInfo(LubNames lubNames, int mLubChargingSpeed, double mLubPrice, string[] mGasType,
            bool mBuyIsActive)
        {
            this.LubNames = lubNames;
            this.MLubChargingSpeed = mLubChargingSpeed;
            this.MLubPrice = mLubPrice;
            this.MGasType = mGasType;
            this.MBuyIsActive = mBuyIsActive;
        }

        public LubInfo(){}


        public LubInfo SetUnitValue(LubNames lubNames)
        {
            LubInfo lubinfo = null;

            switch (lubNames)
            {
                case LubNames.RedLub:
                    {
                        lubinfo = new LubInfo(lubNames, 1, 1000, new string[] {"Diesel", "Gasoline"},true);
                        break;
                        //2023.04.14 웅진 씽크빅 지원으로 일시정지.
                    }
            }
            
            return lubinfo;
        }

}
public class ItemManager : MonoBehaviour
{
}
