//У вас есть программа, которая помогает пользователю составить план поезда.
//Есть 4 основных шага в создании плана:
//-Создать направление - создает направление для поезда(к примеру Бийск - Барнаул)
//-Продать билеты - вы получаете рандомное кол-во пассажиров, которые купили билеты на это направление
//-Сформировать поезд - вы создаете поезд и добавляете ему столько вагонов(вагоны могут быть 
//	разные по вместительности), сколько хватит для перевозки всех пассажиров.
//-Отправить поезд - вы отправляете поезд, после чего можете снова создать направление.
//В верхней части программы должна выводиться полная информация о текущем рейсе или его отсутствии. 

using System.Security.Cryptography.X509Certificates;

namespace Dagny_Taggart_Job
{
	internal class Program
	{
		static void Main(string[] args)
		{
			const string CreateDestination = "1";
			const string SellTickets = "2";
			const string MakeTrain = "3";
			const string CommandTrainToDepart = "4";
			const string CommandExit = "exit";

			Console.WriteLine("Enter для продолжения");

			while (Console.ReadKey().Key != ConsoleKey.Spacebar)
			{
				bool isCommandExit = false;
				bool isPlanCreated = false;
				bool isTrainCreated = false;
				bool isTrafficCreated = false;
				string destinationPoints = "";

				FormationPlan plan = new FormationPlan();
				Traffic traffic = new Traffic();
				Train train = new Train();
				List<Passenger> passengers = new List<Passenger>();

				Console.WriteLine("Если хотите закончить нажмите пробел");

				while (isCommandExit==false)
				{
					Console.WriteLine(
						$"{CreateDestination} - добавить направление\n" +
						$"{SellTickets} - продать билеты\n" +
						$"{MakeTrain} - сформировать состав\n" +
						$"{CommandTrainToDepart} - отправить поезд\n" +
						$"Введите номер команды и нажмите Enter" +
						$"\n выход - exit"
						);
					if (isPlanCreated)
					{
						Console.WriteLine($"Направление: {destinationPoints}, " +
							$"билетов продано:{passengers.Count}");
					}

					string command = Console.ReadLine();

					switch (command)
					{
						case CreateDestination:

							if(isPlanCreated == false)
							{
								destinationPoints = plan.Create();
								isPlanCreated = true;
							}
							else
							{
								Console.WriteLine("Закончите формировать уже созданное направление");
							}

							break;

						case SellTickets:

							if (isTrafficCreated == false & isPlanCreated == true)
							{
								passengers = traffic.PassengerFlow(destinationPoints);
								Console.WriteLine($"Продано {passengers.Count} билетов");
								isTrafficCreated = true;
							}
							else
							{
								Console.WriteLine("Закончите формировать уже созданное направление");
							}
							
							break;

						case MakeTrain:

							if(isTrainCreated == false & isPlanCreated == true & isTrafficCreated ==true)
							{
								train.Make(passengers);
								isTrainCreated = true;
							}
							else
							{
								Console.WriteLine("Закончите формировать уже созданное направление");
							}

							break;

						case CommandTrainToDepart:

							if (isTrainCreated&isTrafficCreated&isPlanCreated)
							{
								Console.WriteLine("Поезд отправляется ЧУ-ЧУУУУ");
								isCommandExit = true;
							}
							else
							{
								Console.WriteLine("Перед отправкой поезда, закончите формировать направление");
							}

							break;

						case CommandExit:
							isCommandExit = true;
							break;

						default:
							Console.WriteLine("Ошибка ввода команды");
							break;
					}

					Console.WriteLine("Нажмите любую кнопку для продолжения");
					Console.ReadKey();
					Console.Clear();
				}
			}
		}
	}

	class Passenger
	{
		private string _destinationTrain;

		public Passenger(string destitationPoints)
		{
			_destinationTrain = destitationPoints;
		}

		public string DestinationTrain
		{
			get
			{
				return _destinationTrain;
			}
			set
			{
				_destinationTrain = value;
			}
		}
	}

	class Train
	{
		private List<Passenger[]> _train = new List<Passenger[]>();
		private Passenger[] _trainCarLarge = new Passenger[16];
		private Passenger[] _trainCarSmall = new Passenger[8];

		public Passenger[] TrainCarLarge
		{
			get
			{
				return _trainCarLarge;
			}
			private set
			{
				_trainCarLarge = value;
			}
		}

		public Passenger[] TrainCarSmall
		{
			get
			{
				return _trainCarSmall;
			}
			private set
			{
				_trainCarSmall =  value;
			}
		}

		public void Make(List<Passenger> passengers)
		{
			int numberOfSmallTrainCars = 0;
			int numberOfLargeTrainCars = 0;
			int actualPassengerNumber = 0;

			if(passengers.Count <= 8)
			{
				numberOfLargeTrainCars = 0;
			}
			else
			{
				numberOfLargeTrainCars = passengers.Count/TrainCarLarge.Length;

				if (passengers.Count%TrainCarLarge.Length>8)
				{
					numberOfLargeTrainCars++;
				}
			}
			
			for(int i = 0; i < numberOfLargeTrainCars; i++)
			{
				for(int j = 0; j < _trainCarLarge.Length; j++)
				{
					_trainCarLarge[i] = passengers[i];
					if (actualPassengerNumber==passengers.Count)
					{
						break;
					}
					actualPassengerNumber++;
				}

				_train.Add(_trainCarLarge);
			}

			if(passengers.Count%TrainCarLarge.Length == 0)
			{
				numberOfSmallTrainCars = 0;
			}
			else
			{
				if(passengers.Count%TrainCarLarge.Length<=8)
				{
					numberOfSmallTrainCars = 1;
				}
			}
			
			int passengersInLastCarTrain = passengers.Count - numberOfLargeTrainCars*TrainCarLarge.Length;

			for (int i = 0; i < numberOfSmallTrainCars; i++)
			{
				for (int j = 0; j < passengersInLastCarTrain;)
				{
					_trainCarSmall[i] = passengers[i];
					actualPassengerNumber++;
					passengersInLastCarTrain--;
				}

				_train.Add(_trainCarSmall);
			}

			ShowInfo();
		}

		public void ShowInfo()
		{
			Console.WriteLine($"поездов в составе {_train.Count}");
		}
	}

	class FormationPlan
	{
		public string Create()
		{
			string[] cities = { "Лондон", "Штормград", "Винтерфел", "Афины", "Нальчик", "Томск", "Рэйвенхольм" };
			string destinationPoints = " ";
			Random random = new Random();

			string departureCity = cities[random.Next(0, cities.Length)];
			destinationPoints = destinationPoints.Insert(0, departureCity);
			destinationPoints = destinationPoints.Insert(destinationPoints.Length, " - ");
			string destinationCity = "";
			bool isAnotherCity = false;
			int randomCityNumber;

			while (isAnotherCity == false)
			{
				for (int i = 0; i < cities.Length; i++)
				{
					randomCityNumber = random.Next(0, cities.Length);

					if (cities[randomCityNumber] != departureCity)
					{
						destinationCity = cities[randomCityNumber];
						isAnotherCity = true;

						break;
					}
				}
			}

			destinationPoints = destinationPoints.Insert(destinationPoints.Length, destinationCity);
			Console.WriteLine(destinationPoints);

			return destinationPoints;
		}
	}
	class Traffic
	{
		public List<Passenger> PassengerFlow(string destitationPoints)
		{
			Random random = new Random();

			List<Passenger> passengers = new List<Passenger>();

			Passenger passenger = new Passenger(destitationPoints); 
			int passengersAmount = random.Next(0, 250);

			for(int i = 0; i < passengersAmount; i++)
			{
				passengers.Add(passenger);
			}

			return passengers;
		}
	}
}