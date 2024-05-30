using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var recipeBook = new RecipeBook();

            while (true)
            {
                Console.WriteLine("Recipe Application");
                Console.WriteLine("1. Add a new recipe");
                Console.WriteLine("2. View a recipe");
                Console.WriteLine("3. Adjust a recipe's quantities");
                Console.WriteLine("4. Reset a recipe's quantities");
                Console.WriteLine("5. Delete a recipe");
                Console.WriteLine("6. List all recipes");
                Console.WriteLine("7. Exit");
                Console.Write("Choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid option. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        recipeBook.AddRecipe();
                        break;
                    case 2:
                        recipeBook.ViewRecipe();
                        break;
                    case 3:
                        recipeBook.AdjustRecipeQuantities();
                        break;
                    case 4:
                        recipeBook.ResetRecipeQuantities();
                        break;
                    case 5:
                        recipeBook.DeleteRecipe();
                        break;
                    case 6:
                        recipeBook.ListRecipes();
                        break;
                    case 7:
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }

    class RecipeBook
    {
        private List<Recipe> recipes = new List<Recipe>();
        public delegate void CalorieWarningHandler(string message);
        public event CalorieWarningHandler CalorieWarning;

        public RecipeBook()
        {
            CalorieWarning += message => Console.WriteLine(message);
        }

        public void AddRecipe()
        {
            var recipe = new Recipe();
            Console.Write("Enter the recipe name: ");
            recipe.Name = Console.ReadLine();
            recipe.AddDetails();
            if (recipe.TotalCalories > 300)
            {
                CalorieWarning?.Invoke($"Warning: {recipe.Name} exceeds 300 calories.");
            }
            recipes.Add(recipe);
            recipes = recipes.OrderBy(r => r.Name).ToList();
        }

        public void ViewRecipe()
        {
            Console.Write("Enter the recipe name to view: ");
            string name = Console.ReadLine();
            var recipe = recipes.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (recipe != null)
            {
                recipe.Display();
            }
            else
            {
                Console.WriteLine("Recipe not found.");
            }
        }

        public void AdjustRecipeQuantities()
        {
            Console.Write("Enter the recipe name to adjust: ");
            string name = Console.ReadLine();
            var recipe = recipes.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (recipe != null)
            {
                recipe.AdjustQuantities();
            }
            else
            {
                Console.WriteLine("Recipe not found.");
            }
        }

        public void ResetRecipeQuantities()
        {
            Console.Write("Enter the recipe name to reset: ");
            string name = Console.ReadLine();
            var recipe = recipes.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (recipe != null)
            {
                recipe.ResetQuantities();
            }
            else
            {
                Console.WriteLine("Recipe not found.");
            }
        }

        public void DeleteRecipe()
        {
            Console.Write("Enter the recipe name to delete: ");
            string name = Console.ReadLine();
            var recipe = recipes.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (recipe != null)
            {
                recipes.Remove(recipe);
                Console.WriteLine("Recipe deleted.");
            }
            else
            {
                Console.WriteLine("Recipe not found.");
            }
        }

        public void ListRecipes()
        {
            if (recipes.Any())
            {
                Console.WriteLine("Recipes:");
                foreach (var recipe in recipes)
                {
                    Console.WriteLine(recipe.Name);
                }
            }
            else
            {
                Console.WriteLine("No recipes available.");
            }
        }
    }

    class Recipe
    {
        public string Name { get; set; }
        private List<Ingredient> ingredients = new List<Ingredient>();
        private List<string> steps = new List<string>();
        public double TotalCalories => ingredients.Sum(i => i.Calories);

        public void AddDetails()
        {
            while (true)
            {
                Console.WriteLine("Enter ingredient details:");
                Console.Write("Ingredient name: ");
                string ingredientName = Console.ReadLine();
                Console.Write("Quantity: ");
                if (!double.TryParse(Console.ReadLine(), out double quantity))
                {
                    Console.WriteLine("Invalid quantity. Please enter a number.");
                    continue;
                }
                Console.Write("Unit: ");
                string unit = Console.ReadLine();
                Console.Write("Calories: ");
                if (!double.TryParse(Console.ReadLine(), out double calories))
                {
                    Console.WriteLine("Invalid calories. Please enter a number.");
                    continue;
                }
                Console.Write("Food group: ");
                string foodGroup = Console.ReadLine();

                ingredients.Add(new Ingredient { Name = ingredientName, Quantity = quantity, Unit = unit, Calories = calories, FoodGroup = foodGroup });

                Console.WriteLine("Enter a step description (or press enter to finish):");
                string step = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(step)) break;
                steps.Add(step);
            }
        }

        public void Display()
        {
            Console.WriteLine($"Recipe: {Name}");
            Console.WriteLine("Ingredients:");
            foreach (var ingredient in ingredients)
            {
                Console.WriteLine($"{ingredient.Quantity} {ingredient.Unit} of {ingredient.Name} ({ingredient.Calories} calories) - {ingredient.FoodGroup}");
            }
            Console.WriteLine($"\nTotal Calories: {TotalCalories}");
            if (TotalCalories > 300)
            {
                Console.WriteLine("Warning: This recipe exceeds 300 calories.");
            }

            Console.WriteLine("\nSteps:");
            for (int i = 0; i < steps.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {steps[i]}");
            }
        }

        public void AdjustQuantities()
        {
            Console.WriteLine("Enter the scaling factor:");
            if (!double.TryParse(Console.ReadLine(), out double factor))
            {
                Console.WriteLine("Invalid scaling factor. Please enter a number.");
                return;
            }

            foreach (var ingredient in ingredients)
            {
                ingredient.Quantity *= factor;
                ingredient.Calories *= factor;
            }
        }

        public void ResetQuantities()
        {
            foreach (var ingredient in ingredients)
            {
                ingredient.Quantity = 0;
            }
        }

        public void Clear()
        {
            ingredients.Clear();
            steps.Clear();
            Console.WriteLine("Recipe cleared.");
        }
    }

    class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double Calories { get; set; }
        public string FoodGroup { get; set; }
    }
}


