using System.Net.Http.Json;

namespace BinanceP2pAlerts;
internal class BinanceClient(HttpClient _http)
{
    /// <summary>
    /// First ad has the lowest price when BUYing.
    /// </summary>
    public async Task<Ad> GetFirstAd()
    {
        const string json = """
			   {
			    "fiat": "AED",
			    "page": 1,
			    "rows": 1,
			    "tradeType": "BUY",
			    "asset": "USDT",
			    "countries": [],
			    "proMerchantAds": false,
			    "shieldMerchantAds": false,
			    "filterType": "all",
			    "additionalKycVerifyFilter": 0,
			    "publisherType": null,
			    "payTypes": ["BANK", "SpecificBank"],
			    "classifies": [
			        "mass",
			        "profession"
			    ]
			}
			""";

        using HttpRequestMessage req = new(HttpMethod.Post, "bapi/c2c/v2/friendly/c2c/adv/search")
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };

        var resp = await _http.SendAsync(req);
        var respContent = await resp.Content.ReadFromJsonAsync<BinanceResponse>(AppJsonSerializerContext.Default.BinanceResponse);
        var price = decimal.Parse(respContent!.data.Single().adv.price);
        return new(price);
    }
}

#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
internal class BinanceResponse
{
    public string code { get; set; }
    public object message { get; set; }
    public object messageDetail { get; set; }
    public Datum[] data { get; set; }
    public int total { get; set; }
    public bool success { get; set; }
}

internal class Datum
{
    public Adv adv { get; set; }
    public Advertiser advertiser { get; set; }
}

internal class Adv
{
    public string advNo { get; set; }
    public string classify { get; set; }
    public string tradeType { get; set; }
    public string asset { get; set; }
    public string fiatUnit { get; set; }
    public object advStatus { get; set; }
    public object priceType { get; set; }
    public object priceFloatingRatio { get; set; }
    public object rateFloatingRatio { get; set; }
    public object currencyRate { get; set; }
    public string price { get; set; }
    public object initAmount { get; set; }
    public string surplusAmount { get; set; }
    public string tradableQuantity { get; set; }
    public object amountAfterEditing { get; set; }
    public string maxSingleTransAmount { get; set; }
    public string minSingleTransAmount { get; set; }
    public object buyerKycLimit { get; set; }
    public object buyerRegDaysLimit { get; set; }
    public object buyerBtcPositionLimit { get; set; }
    public object remarks { get; set; }
    public string autoReplyMsg { get; set; }
    public int payTimeLimit { get; set; }
    public Trademethod[] tradeMethods { get; set; }
    public object userTradeCountFilterTime { get; set; }
    public object userBuyTradeCountMin { get; set; }
    public object userBuyTradeCountMax { get; set; }
    public object userSellTradeCountMin { get; set; }
    public object userSellTradeCountMax { get; set; }
    public object userAllTradeCountMin { get; set; }
    public object userAllTradeCountMax { get; set; }
    public object userTradeCompleteRateFilterTime { get; set; }
    public object userTradeCompleteCountMin { get; set; }
    public object userTradeCompleteRateMin { get; set; }
    public object userTradeVolumeFilterTime { get; set; }
    public object userTradeType { get; set; }
    public object userTradeVolumeMin { get; set; }
    public object userTradeVolumeMax { get; set; }
    public object userTradeVolumeAsset { get; set; }
    public object createTime { get; set; }
    public object advUpdateTime { get; set; }
    public object fiatVo { get; set; }
    public object assetVo { get; set; }
    public object advVisibleRet { get; set; }
    public int takerAdditionalKycRequired { get; set; }
    public object inventoryType { get; set; }
    public object offlineReason { get; set; }
    public object assetLogo { get; set; }
    public int assetScale { get; set; }
    public int fiatScale { get; set; }
    public int priceScale { get; set; }
    public string fiatSymbol { get; set; }
    public bool isTradable { get; set; }
    public string dynamicMaxSingleTransAmount { get; set; }
    public string minSingleTransQuantity { get; set; }
    public string maxSingleTransQuantity { get; set; }
    public string dynamicMaxSingleTransQuantity { get; set; }
    public string commissionRate { get; set; }
    public object takerCommissionRate { get; set; }
    public object minTakerFee { get; set; }
    public object[] tradeMethodCommissionRates { get; set; }
    public object launchCountry { get; set; }
    public object abnormalStatusList { get; set; }
    public object closeReason { get; set; }
    public object storeInformation { get; set; }
    public object allowTradeMerchant { get; set; }
}

internal class Trademethod
{
    public object payId { get; set; }
    public string payMethodId { get; set; }
    public object payType { get; set; }
    public object payAccount { get; set; }
    public object payBank { get; set; }
    public object paySubBank { get; set; }
    public string identifier { get; set; }
    public object iconUrlColor { get; set; }
    public string tradeMethodName { get; set; }
    public string tradeMethodShortName { get; set; }
    public string tradeMethodBgColor { get; set; }
}

internal class Advertiser
{
    public string userNo { get; set; }
    public object realName { get; set; }
    public string nickName { get; set; }
    public object margin { get; set; }
    public object marginUnit { get; set; }
    public object orderCount { get; set; }
    public int monthOrderCount { get; set; }
    public float monthFinishRate { get; set; }
    public float positiveRate { get; set; }
    public object advConfirmTime { get; set; }
    public object email { get; set; }
    public object registrationTime { get; set; }
    public object mobile { get; set; }
    public string userType { get; set; }
    public object[] tagIconUrls { get; set; }
    public int userGrade { get; set; }
    public string userIdentity { get; set; }
    public object proMerchant { get; set; }
    public string[] badges { get; set; }
    public object isBlocked { get; set; }
    public int activeTimeInSecond { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning restore IDE1006 // Naming Styles
