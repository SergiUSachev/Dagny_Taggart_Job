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
			FormationPlan plan = new FormationPlan();
			Traffic traffic = new Traffic();
			Train train = new Train();

			plan.Create();
			List<Passenger> passengers = traffic.PassengerFlow();
			Console.WriteLine(passengers.Count);

			train.MakeTrain(passengers);

			train.Show();

		}
	}

	class Passenger
	{
		private int TrainCar;
		private int DestinationTrain;

		public Passenger(int trainCar, int destinationTrain)
		{
			TrainCar=trainCar;
			DestinationTrain=destinationTrain;
		}

		public Passenger()
		{

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
		}

		public Passenger[] TrainCarSmall
		{
			get
			{
				return _trainCarSmall;
			}
			set
			{
				_trainCarSmall = value;
			}
		}

		public void MakeTrain(List<Passenger> passengers)
		{

			int numberOfSmallTrainCars = 0;
			int numberOfLargeTrainCars = 0;
			int actualPassengerNumber = 0;

			numberOfLargeTrainCars = passengers.Count/TrainCarLarge.Length;

			for(int i = 0; i < numberOfLargeTrainCars; i++)
			{
				for(int j = 0; j < _trainCarLarge.Length; j++)
				{
					_trainCarLarge[i] = passengers[i];
					actualPassengerNumber++;
				}
				_train.Add(_trainCarLarge);
			}

			if (passengers.Count%TrainCarLarge.Length>8)
			{
				numberOfSmallTrainCars = 2;
			}
			else
			{
				if(passengers.Count%TrainCarLarge.Length == 0)
				{
					numberOfSmallTrainCars = 0;
				}
				else
				{
					numberOfSmallTrainCars = 1;
				}
			}

			for (int i = 0; i < numberOfSmallTrainCars; i++)
			{
				for (int j = 0; j < _trainCarSmall.Length; j++)
				{
					_trainCarSmall[i] = passengers[i];
					actualPassengerNumber++;
				}
				_train.Add(_trainCarSmall);
			}
			
		}

		public void Show()
		{
			foreach(var carTrain in _train)
			{
				Console.WriteLine(carTrain);
			}
			Console.WriteLine(_train.Count);
		}
	}

	class FormationPlan
	{
		public void Create()
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
		}
	}
	class Traffic
	{
		public List<Passenger> PassengerFlow()
		{
			Random random = new Random();

			List<Passenger> passengers = new List<Passenger>();

			Passenger passenger = new Passenger(); 
			int passengersAmount = random.Next(33, 50);

			for(int i = 0; i < passengersAmount; i++)
			{
				passengers.Add(passenger);
			}

			return passengers;
		}
	}
}