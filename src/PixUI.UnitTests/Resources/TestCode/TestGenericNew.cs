interface ISeries<T> where T : new()
{
    //T Create() => new T();
}

class Model { }

abstract class SeriesBase<T> : ISeries<T> where T : new()
{
    T MakeModel() => new T();
    //注入构造工厂方案:
    // MakeModel() { return this.__typeFactory["SeriesBase.T"](); }
}

class ChartSeries<TModel> : SeriesBase<TModel> where TModel : new()
{
    TModel MakeChartModel() => new TModel();
    //注入构造工厂方案:
    // MakeChartModel() { return this.__typeFactory["ChartSeries.TModel"](); }
}

class LineSeries: ChartSeries<Model> { }

class TestClass
{
    void Test()
    {
        var obj = new LineSeries();
        //注入构造工厂方案:
        // let obj = new LineSeries().InjectFactory([
        //    { "ChartSeries.TModel", () => new Model() },
        //    { "SeriesBase.T", () => new Model() },
        // ]);
    }
}