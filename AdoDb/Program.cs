using AdoDb;

namespace AdoDb
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var repo = new UserRepository();

            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Добавить нового пользователя");
                Console.WriteLine("2. Обновить данные пользователя");
                Console.WriteLine("3. Найти пользователя");
                Console.WriteLine("4. Удалить пользователя");
                Console.WriteLine("5. Показать всех пользователей");
                Console.WriteLine("6. Завершить выполнение программы");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Неверный формат ввода");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Введите имя");
                        string name = Console.ReadLine() ?? string.Empty;

                        Console.WriteLine("Введите почту");
                        string email = Console.ReadLine() ?? string.Empty;

                        Console.WriteLine("Введите возраст");
                        if (!int.TryParse(Console.ReadLine(), out int age))
                        {
                            Console.WriteLine("Неверный формат возраста");
                            break;
                        }

                        var newUser = new User { Name = name, Email = email, Age = age };
                        await repo.CreateAsync(newUser);
                        Console.WriteLine("Пользователь успешно добавлен");
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("Введите ID пользователя для обновления:");

                        if (!int.TryParse(Console.ReadLine(), out int updateId))
                        {
                            Console.WriteLine("Неверный формат ID");
                            break;
                        }

                        var existingUser = await repo.GetByIdAsync(updateId);
                        if (existingUser != null)
                        {
                            Console.WriteLine("Введите новое имя:");
                            string newName = Console.ReadLine() ?? string.Empty;

                            Console.WriteLine("Введите новую почту:");
                            string newEmail = Console.ReadLine() ?? string.Empty;

                            Console.WriteLine("Введите новый возраст:");
                            if (!int.TryParse(Console.ReadLine(), out int newAge))
                            {
                                Console.WriteLine("Неверный формат возраста");
                                break;
                            }

                            existingUser.Name = newName;
                            existingUser.Email = newEmail;
                            existingUser.Age = newAge;

                            await repo.UpdateAsync(existingUser);
                            Console.WriteLine("Пользователь успешно обновлен");
                        }
                        else
                        {
                            Console.WriteLine("Пользователь не найден");
                        }
                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("Введите ID пользователя для поиска:");

                        if (!int.TryParse(Console.ReadLine(), out int searchId))
                        {
                            Console.WriteLine("Неверный формат ID");
                            break;
                        }

                        var foundUser = await repo.GetByIdAsync(searchId);
                        if (foundUser != null)
                        {
                            Console.WriteLine($"ID: {foundUser.Id}, Имя: {foundUser.Name}, Email: {foundUser.Email}, Возраст: {foundUser.Age}");
                        }
                        else
                        {
                            Console.WriteLine("Пользователь не найден");
                        }
                        break;

                    case 4:
                        Console.Clear();
                        Console.WriteLine("Введите ID пользователя для удаления:");

                        if (!int.TryParse(Console.ReadLine(), out int deleteId))
                        {
                            Console.WriteLine("Неверный формат ID");
                            break;
                        }

                        bool exists = await repo.ExistsAsync(deleteId);
                        if (exists)
                        {
                            await repo.DeleteAsync(deleteId);
                            Console.WriteLine("Пользователь успешно удален");
                        }
                        else
                        {
                            Console.WriteLine("Пользователь не найден");
                        }
                        break;

                    case 5:
                        Console.Clear();
                        var users = await repo.GetAllAsync();

                        if (users.Count > 0)
                        {
                            Console.WriteLine("Список всех пользователей:");
                            foreach (var user in users)
                            {
                                Console.WriteLine($"ID: {user.Id}, Имя: {user.Name}, Email: {user.Email}, Возраст: {user.Age}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Список пользователей пуст");
                        }
                        break;

                    case 6:
                        Console.WriteLine("Программа завершена");
                        return;

                    default:
                        Console.WriteLine("Неверный выбор. Пожалуйста, выберите от 1 до 6");
                        break;
                }
            }
        }
    }
}