using System;
using System.Collections.Generic;

namespace SinSity.Core {
    public interface ICardsCollection {
        ICard GetCard(string id);
        bool HasCard(string id);
        IEnumerable<ICard> GetAllCards();
    }
}