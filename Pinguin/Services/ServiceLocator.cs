using System;

namespace Pinguin.Services;

public static class ServiceLocator
{
    public static IServiceProvider Instance { get; set; }
}