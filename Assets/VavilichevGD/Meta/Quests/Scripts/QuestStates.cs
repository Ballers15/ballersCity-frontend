using System;
using System.Collections.Generic;

namespace VavilichevGD.Meta {
    [Serializable]
    public class QuestStates {
        public List<string> listOfStates;

        public static QuestStates empty {
            get {
                QuestStates states = new QuestStates();
                return states;
            }
        }

        public QuestStates() {
            listOfStates = new List<string>();
        }

        public QuestStates(QuestState[] statesArray) {
            listOfStates = new List<string>();
            foreach (QuestState state in statesArray) {
                string stateJson = state.GetStateJson();
                listOfStates.Add(stateJson);
            }
        }
    }
}