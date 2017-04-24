using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperTest
{
    [HtmlExporter]
  public  class MapperBenchmarking
    {
        [Params(1, 10, 100)]
        public int Param { get; set; }

        private readonly List<User> _userList;
        private readonly MapperConfiguration _config;
        public MapperBenchmarking()
        {
            #region AutoMapper
            _config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDto>());
            AutoMapper.Mapper.Initialize(cfg => cfg.CreateMap<User, UserDto>());
            #endregion

            _userList = new List<User>();
        }

        [Setup]
        public void Setup()
        {
            for (int i = 0; i < Param; i++)
            {
                _userList.Add(new User
                {
                    Address = Faker.Address.SecondaryAddress(),
                    City = Faker.Address.City(),
                    Age = Faker.RandomNumber.Next(10, 100),
                    Name = Faker.Name.First(),
                    Surname = Faker.Name.Last(),
                    Phone = Faker.Phone.Number(),
                    Web = Faker.Internet.DomainName(),
                    Email = Faker.Internet.Email(),
                    Country = Faker.Address.Country()
                });
            }
        }

        [Benchmark]
        public void AutoMapperBenchmark()
        {
            var userDtoList = new List<UserDto>();
            var config = _config.CreateMapper();
            foreach (var user in _userList)
            {
                userDtoList.Add(config.Map<UserDto>(user));
            }
        }

        [Benchmark]
        public void WithLinqBenchmark()
        {
            var dtoLinq = _userList.Select(T => new UserDto
            {
                Name = T.Name,
                Surname = T.Surname
            }).ToArray();
        }

        [Benchmark]
        public void TinyMapperBenchmark()
        {
            var userDtoList = new List<UserDto>();
            foreach (var user in _userList)
            {
                userDtoList.Add(TinyMapper.Map<UserDto>(user));
            }
        }

        [Benchmark]
        public void ExpressMapperBencmark()
        {
            var userDtoList = new List<UserDto>();
            foreach (var user in _userList)
            {
                userDtoList.Add(ExpressMapper.Mapper.Map<User, UserDto>(user));
            }
        }
    }
}
