namespace SinSity.Core {
    public struct CharacterConstructParams {
        public ICharacterInfo info;
        public IIdleObject idleObject;
        public ICard card;

        public CharacterConstructParams(ICharacterInfo info, IIdleObject characterIdleObject, ICard characterCard) {
            this.info = info;
            idleObject = characterIdleObject;
            card = characterCard;
        }
    }
}