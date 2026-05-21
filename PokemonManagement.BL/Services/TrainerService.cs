using PokemonManagement.BL.Exceptions;
using PokemonManagement.BL.Interfaces;
using PokemonManagement.DAL.Models;
using PokemonManagement.DAL.Repositories;

namespace PokemonManagement.BL.Services
{
    public class TrainerService : ITrainerService
    {
        private readonly ITrainerPokemonRepository _repository;

        public TrainerService(ITrainerPokemonRepository repository)
        {
            _repository = repository;
        }

        public void Train(int trainerPokemonId)
        {
            // 1 retrieve ownedpokemon by trainerPokemonId
            var ownedPokemon = _repository.GetBy(trainerPokemonId);
            if (ownedPokemon is null) 
                throw new EntityNotFoundException();

            // 2 check valid level
            if (ownedPokemon.Level >= 100)
                throw new PokemonLogicException("max level reached");

            // 3 increase level and update entity
            ownedPokemon.Level += 1;
            _repository.Update(ownedPokemon);
        }

        public void Evolve(int trainerPokemonId)
        {
            var owndPkmn = _repository.GetBy(trainerPokemonId);
            if(owndPkmn is null) throw new EntityNotFoundException();

            assertPokemonLevel(owndPkmn);
            assertTrainerCandies(owndPkmn);
            assertNextEvolution(owndPkmn);

            owndPkmn.PokemonId = (int) owndPkmn.Pokemon.EvolvesToId;
            _repository.Update(owndPkmn);
        }

        private void assertNextEvolution(TrainerPokemon owndPkmn)
        {
            if (owndPkmn.Pokemon.EvolvesToId is null)
                throw new PokemonLogicException("Pokemon no evolve");
        }

        private void assertTrainerCandies(TrainerPokemon owndPkmn)
        {
            if (owndPkmn.Trainer.Candies < 50)
                throw new PokemonLogicException("No candies");
        }

        private void assertPokemonLevel(TrainerPokemon owndPkmn)
        {
            if (owndPkmn.Level < 16)
                throw new PokemonLogicException("Pokemon is not level 16 or higher");
        }
    }
}
