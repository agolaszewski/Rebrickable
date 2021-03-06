using Newtonsoft.Json;
using System;
using System.Collections.Generic;

//https://json2csharp.com/
namespace RebrickableApi
{
    public record LegoSetsListAsyncResponse(
        [property: JsonProperty("count")] int Count,
        [property: JsonProperty("next")] string Next,
        [property: JsonProperty("previous")] string Previous,
        [property: JsonProperty("results")] IReadOnlyList<LegoSetsListAsyncResponse.Result> Results)
    {
        public record Result(
            [property: JsonProperty("set_num")] string SetNum,
            [property: JsonProperty("name")] string Name,
            [property: JsonProperty("year")] int Year,
            [property: JsonProperty("theme_id")] int ThemeId,
            [property: JsonProperty("num_parts")] int NumParts,
            [property: JsonProperty("set_img_url")] string SetImgUrl,
            [property: JsonProperty("set_url")] string SetUrl,
            [property: JsonProperty("last_modified_dt")] DateTime LastModifiedDt
        );
    }

    public record LegoThemesListAsyncResponse(
        [property: JsonProperty("count")] int Count,
        [property: JsonProperty("next")] string Next,
        [property: JsonProperty("previous")] object Previous,
        [property: JsonProperty("results")] IReadOnlyList<LegoThemesListAsyncResponse.Result> Results)
    {
        public record Result(
            [property: JsonProperty("id")] int Id,
            [property: JsonProperty("parent_id")] int? ParentId,
            [property: JsonProperty("name")] string Name
        );
    }
}