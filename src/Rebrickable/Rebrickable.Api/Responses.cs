using System;
using System.Collections.Generic;
using Newtonsoft.Json;

//https://json2csharp.com/
namespace Rebrickable.Api
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
        [property: JsonProperty("previous")] string Previous,
        [property: JsonProperty("results")] IReadOnlyList<LegoThemesListAsyncResponse.Result> Results)
    {
        public record Result(
            [property: JsonProperty("id")] int Id,
            [property: JsonProperty("parent_id")] int? ParentId,
            [property: JsonProperty("name")] string Name
        );
    }

    public record LegoSetsReadAsyncResponse(
        [property: JsonProperty("set_num")] string SetNum,
        [property: JsonProperty("name")] string Name,
        [property: JsonProperty("year")] int Year,
        [property: JsonProperty("theme_id")] int ThemeId,
        [property: JsonProperty("num_parts")] int NumParts,
        [property: JsonProperty("set_img_url")] string SetImgUrl,
        [property: JsonProperty("set_url")] string SetUrl,
        [property: JsonProperty("last_modified_dt")] DateTime LastModifiedDt
    );

    public record LegoSetsMinifigsListAsyncResponse(
        [property: JsonProperty("count")] int Count,
        [property: JsonProperty("next")] string Next,
        [property: JsonProperty("previous")] string Previous,
        [property: JsonProperty("results")] IReadOnlyList<LegoSetsMinifigsListAsyncResponse.Result> Results)
    {
        public record Result(
            [property: JsonProperty("id")] int Id,
            [property: JsonProperty("set_num")] string SetNum,
            [property: JsonProperty("set_name")] string SetName,
            [property: JsonProperty("quantity")] int Quantity,
            [property: JsonProperty("set_img_url")] string SetImgUrl
        );
    }

    public record LegoMinifigsSetsListAsyncResponse(
        [property: JsonProperty("count")] int Count,
        [property: JsonProperty("next")] string Next,
        [property: JsonProperty("previous")] string Previous,
        [property: JsonProperty("results")] IReadOnlyList<LegoMinifigsSetsListAsyncResponse.Result> Results)
    {
        public record Result(
            [property: JsonProperty("id")] int Id,
            [property: JsonProperty("set_num")] string SetNum,
            [property: JsonProperty("set_name")] string SetName,
            [property: JsonProperty("quantity")] int Quantity,
            [property: JsonProperty("set_img_url")] string SetImgUrl
        );
    }
}