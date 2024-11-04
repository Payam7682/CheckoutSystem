using System;
using System.Collections.Generic;
using CheckoutSystem.Core.Interfaces;
using CheckoutSystem.Core.Models;
using CheckoutSystem.Core.Services;
using CheckoutSystem.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

// Set up dependency injection
var serviceProvider = new ServiceCollection()
    .AddScoped<IItemRepository, ItemRepository>()
    .AddSingleton<PricingRepository>()
    .AddScoped<IPricingService>(sp =>
    {
        var repo = sp.GetRequiredService<PricingRepository>();
        return new PricingService(repo.GetPricingRules());
    })
    .AddScoped<ICheckoutService, CheckoutService>()
    .BuildServiceProvider();

// Get checkout service
var checkoutService = serviceProvider.GetService<ICheckoutService>();

Console.WriteLine("Welcome to the Checkout System!");
Console.WriteLine("Available items: A (50 cents, 3 for 130), B (30 cents, 2 for 45), C (20 cents), D (15 cents)");
Console.WriteLine("Type 'exit' to finish checkout and get the total.");

string input;
while (true)
{

    Console.Write("Scan item (A, B, C, D): ");
    input = Console.ReadLine()??"exit";

    if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    if (input is "A" or "B" or "C" or "D")
    { 
        if (!string.IsNullOrEmpty(input))
        {
            checkoutService!.Scan(input);
            Console.WriteLine($"Item {input} scanned.");
        }
    else 
        {
            Console.WriteLine("Invalid input. Please enter a valid SKU or 'exit' to finish.");
        }
    }
    else
    {
        Console.WriteLine("Invalid item. Please scan a valid item (A, B, C, D).");
    }
}

// Get the total price
decimal total = checkoutService!.GetTotal();
Console.WriteLine($"Total price: {total} $");

Console.WriteLine("Thank you for shopping with us!");
