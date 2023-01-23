using System;
using AeroMapPresentor.Core.Models;

namespace AeroMapPresenter.Mvvm.Wpf;

public interface IMapEntitiesProvider
{
    event EventHandler<MapEntity> EntityAdded;
}

public class MapEntitiesProvider : IMapEntitiesProvider
{
    public event EventHandler<MapEntity>? EntityAdded;
}