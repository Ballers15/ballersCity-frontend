using System;
using System.Collections.Generic;
using System.Linq;
using SinSity.Core;
using SinSity.Repo;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Domain {
    public class CharactersInteractor : Interactor {
        public event Action<List<CharacterReward>> OnRewardsGenerated;
        
        private IdleObjectsInteractor _idleObjectsInteractor;
        private CardsInteractor _cardsInteractor;
        private List<ICharacter> _characters;
        private ICharacterInfo[] _characterInfos;
        private CharacterRewardsGenerator _characterRewardsGenerator;
        
        public override bool onCreateInstantly { get; } = true;

        public List<CharacterReward> GenerateCharactersRewards() {
            var rewards = _characterRewardsGenerator.Generate();
            OnRewardsGenerated?.Invoke(rewards);
            return rewards;
        }

        public ICharacter GetCharacter(string id) {
            return _characters.First(character => character.id == id);
        }

        public List<ICharacter> GetAllCharacters() {
            return _characters;
        }

        public int GetCharactersCount() {
            return _characters.Count;
        }

        public List<ICharacter> GetActiveCharacters() {
            return _characters.Where(character => character.isActive).ToList();
        }
        
        public List<ICharacter> GetCharactersForIdle(string idleObjectId) {
            return _characters.Where(character => character.idleObject.id == idleObjectId).ToList();
        }

        #region INITIALIZATION

        protected override void Initialize() {
            base.Initialize();

            InitializeRequiredInteractors();
            LoadCharactersInfos();
            InitializeCharacters();
            SortCharacters();
            InitializeCharacterControllers();
        }

        private void InitializeRequiredInteractors() {
            _idleObjectsInteractor = GetInteractor<IdleObjectsInteractor>();
            _cardsInteractor = GetInteractor<CardsInteractor>();
        }
        
        private void LoadCharactersInfos() {
            using var charactersInfoLoader = new CharactersInfoLoader();
            _characterInfos = charactersInfoLoader.Load();
        }

        #region InitializeCharacters

        private void InitializeCharacters() {
            _characters = new List<ICharacter>();
            foreach (var characterInfo in _characterInfos) {
                var character = CreateCharacter(characterInfo);
                _characters.Add(character);
            }
        }

        private ICharacter CreateCharacter(ICharacterInfo characterInfo) {
            var characterIdle = _idleObjectsInteractor.GetIdleObject(characterInfo.idleObjectInfo.id);
            var characterCard = _cardsInteractor.GetCard(characterInfo.characterCardInfo.id);
            var characterConstructParams = new CharacterConstructParams(characterInfo,characterIdle,characterCard);
            return new Character(characterConstructParams);
        }

        #endregion
        
        private void SortCharacters() {
            _characters = _characters.OrderBy(character => character.characterCard.GetSortingOrder()).ToList();
        }

        private void InitializeCharacterControllers() {
            var characterIncomeMultipliersApplier = new CharactersIdleObjectIncomeMultiplierController(this);
            characterIncomeMultipliersApplier.Initialize();
            _characterRewardsGenerator = new CharacterRewardsGenerator(this);
        }

        #endregion
    }
}