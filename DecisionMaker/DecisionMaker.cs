using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecisionMaker
{
    class Decision
    {
        public string DecisionName { get; set; }
        public DateTime DecisionStartTime { get; set; }
        public DateTime DecisionMadeTime { get; set; }

        public LinkedList<Option> OptionsList
        {
            get
            {
                return optionsList;
            }
        }

        public LinkedList<Criteria> CriteriaList
        {
            get
            {
                return criteriaList;
            }
        }

        private LinkedList<Option> optionsList = null;

        private LinkedList<Criteria> criteriaList = null;

        public Decision(string decisionName)
        {
            DecisionName = decisionName;
            DecisionStartTime = DateTime.Now;

            if (optionsList == null)
            {
                optionsList = new LinkedList<Option>();
            }

            if (criteriaList == null)
            {
                criteriaList = new LinkedList<Criteria>();
            }
        }

        public Option addOption(string optionName)
        {
            if (GivenOptionExists(optionName))
            {
                throw new Exception(string.Format("{0} exists already, please give it another name or delete the old one first", optionName));
            }
            else
            {
                Option option = new Option(optionName);
                option.UpdateCriteriaInOption(CriteriaList);
                optionsList.AddLast(option);
                return option;
            }
        }

        public void deleteOption(string optionName)
        {
            if (GivenOptionExists(optionName))
            {
                optionsList.Remove(optionsList.LastOrDefault(d => d.OptionName.ToLower() == optionName.ToLower()));
            }
            else
            {
                throw new Exception(string.Format("{0} does not exist", optionName));
            }
        }

        public void updateOption(string oldOptionName, Option newOption)
        {
            if (GivenOptionExists(oldOptionName))
            {
                optionsList.LastOrDefault(d => d.OptionName.ToLower() == oldOptionName.ToLower()).Description = newOption.Description;
                optionsList.LastOrDefault(d => d.OptionName.ToLower() == oldOptionName.ToLower()).Link = newOption.Link;

                optionsList.LastOrDefault(d => d.OptionName.ToLower() == oldOptionName.ToLower()).OptionName = newOption.OptionName;
            }
            else
            {
                throw new Exception(string.Format("{0} does not exist", oldOptionName));
            }
        }

        public Option GetOptionByName(string optionName)
        {
            return OptionsList.FirstOrDefault(o => o.OptionName.ToLower() == optionName.ToLower());
        }

        public void addCriteria(string CriteriaName, int weight = 0)
        {
            if (GivenCriteriaExists(CriteriaName))
            {
                throw new Exception(string.Format("{0} exists already, please give it another name or delete the old one first", CriteriaName));
            }
            else
            {
                CriteriaList.AddLast(new Criteria(CriteriaName, weight));
            }

            UpdateCriteriaInAllOptions();
        }

        public void deleteCriteria(string CriteriaName)
        {
            if (GivenCriteriaExists(CriteriaName))
            {
                CriteriaList.Remove(CriteriaList.LastOrDefault(d => d.CriteriaName.ToLower() == CriteriaName.ToLower()));

                UpdateCriteriaInAllOptions();
            }
            else
            {
                throw new Exception(string.Format("{0} does not exist", CriteriaName));
            }
        }

        public void updateCriteria(string oldCriteriaName, Criteria newCriteria)
        {
            if (GivenCriteriaExists(oldCriteriaName))
            {
                CriteriaList.LastOrDefault(d => d.CriteriaName.ToLower() == oldCriteriaName.ToLower()).Weight = newCriteria.Weight;
                // CriteriaList.LastOrDefault(d => d.CriteriaName.ToLower() == oldCriteriaName.ToLower()).Rating = newCriteria.Rating;
                // CriteriaList.LastOrDefault(d => d.CriteriaName.ToLower() == oldCriteriaName.ToLower()).Value = newCriteria.Value;
                CriteriaList.LastOrDefault(d => d.CriteriaName.ToLower() == oldCriteriaName.ToLower()).CriteriaName = newCriteria.CriteriaName;

                UpdateCriteriaInAllOptions(oldCriteriaName);
            }
            else
            {
                throw new Exception(string.Format("{0} does not exist", oldCriteriaName));
            }
        }

        public Criteria GetCriteriaByName(string criteriaName)
        {
            return CriteriaList.FirstOrDefault(c => c.CriteriaName.ToLower() == criteriaName.ToLower());
        }

        public string CalculateAndGetResults()
        {
            string output = string.Empty;

            foreach (Option option in OptionsList)
            {
                option.CalculateResult();
                output += option.OptionName + ": " + option.Point + Environment.NewLine;
            }

            return output;
        }

        public void ValidateResults(params int[] expectedResults)
        {
            string errorMessage = string.Empty;

            if (expectedResults.Length == OptionsList.Count)
            {
                int i = 0;
                foreach (Option option in OptionsList)
                {
                    option.CalculateResult();
                    if (option.Point != expectedResults[i])
                    {
                        errorMessage += string.Format("Expected option {0} to have points {1}, but it was {2}\n", option.OptionName, expectedResults[i], option.Point);
                    }

                    i++;
                }
            }
            else
            {
                errorMessage += string.Format("Expected option count to be {0}, but it was {1}\n", expectedResults.Length, OptionsList.Count);
            }

            if (errorMessage != string.Empty)
            {
                throw new Exception(errorMessage);
            }
        }

        private bool GivenOptionExists(string optionName)
        {
            return optionsList.Any(d => d.OptionName.ToLower() == optionName.ToLower());
        }

        private bool GivenCriteriaExists(string CriteriaName)
        {
            return CriteriaList.Any(d => d.CriteriaName.ToLower() == CriteriaName.ToLower());
        }

        private void UpdateCriteriaInAllOptions(string updatingCriteriaName = null)
        {
            foreach (Option option in optionsList)
            {
                option.UpdateCriteriaInOption(CriteriaList, updatingCriteriaName);
            }
        }
    }

    class Option
    {
        public string OptionName { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public int Point { get; set; }

        public LinkedList<Criteria> CriteriaList
        {
            get
            {
                return criteriaList;
            }
        }

        public LinkedList<Criteria> criteriaList = null;

        public Option(string name, string description = null, string link = null)
        {
            OptionName = name;
            Description = description;
            Link = link;
            Point = 0;

            if (criteriaList == null)
            {
                criteriaList = new LinkedList<Criteria>();
            }
        }

        public void SetCriteriaRatingAndValue(string criteriaName, int rating, string value = null)
        {
            if (GivenCriteriaExists(criteriaName))
            {
                Criteria foundCriteria = criteriaList.LastOrDefault(d => d.CriteriaName.ToLower() == criteriaName.ToLower());
                foundCriteria.Rating = rating;
                foundCriteria.Value = value;
            }
            else
            {
                throw new Exception(string.Format("{0} does not exist", criteriaName));
            }
        }

        public void UpdateCriteriaInOption(LinkedList<Criteria> parentCriteriaList, string updatingCriteriaName = null)
        {
            foreach (Criteria criteria in parentCriteriaList)
            {
                if (GivenCriteriaExists(criteria.CriteriaName))
                {
                    criteria.Rating = CriteriaList.First(d => d.CriteriaName.ToLower() == criteria.CriteriaName.ToLower()).Rating;
                    criteria.Value = CriteriaList.First(d => d.CriteriaName.ToLower() == criteria.CriteriaName.ToLower()).Value;
                }
                else if (updatingCriteriaName != null)
                {
                    if (GivenCriteriaExists(updatingCriteriaName))
                    {
                        criteria.Rating = CriteriaList.First(d => d.CriteriaName.ToLower() == updatingCriteriaName.ToLower()).Rating;
                        criteria.Value = CriteriaList.First(d => d.CriteriaName.ToLower() == updatingCriteriaName.ToLower()).Value;
                    }
                }
            }

            CriteriaList.Clear();
            foreach (Criteria criteria in parentCriteriaList)
            {
                CriteriaList.AddLast(new Criteria(criteria.CriteriaName, criteria.Weight, criteria.Rating, criteria.Value));

                // Clean parent list for to pretent not touching it anyhow
                criteria.Rating = 0;
                criteria.Value = null;
            }
        }

        public void CalculateResult()
        {
            Point = 0;
            foreach (Criteria criteria in CriteriaList)
            {
                Point += criteria.Weight * criteria.Rating;
            }
        }

        private bool GivenCriteriaExists(string criteriaName)
        {
            return CriteriaList.Any(d => d.CriteriaName.ToLower() == criteriaName.ToLower());
        }
    }

    class Criteria
    {
        public string CriteriaName { get; set; }
        public int Rating { get; set; }
        public string Value { get; set; }
        public int Weight { get; set; }

        public Criteria(string name, int weight = 0)
        {
            CriteriaName = name;
            Weight = weight;
        }

        public Criteria(string name, int weight, int rating, string value)
        {
            CriteriaName = name;
            Weight = weight;
            Rating = rating;
            Value = value;
        }
    }

    class DecisionMaker
    {
        private LinkedList<Decision> decisionsList = null;

        public LinkedList<Decision> DecisionsList
        {
            get
            {
                return decisionsList;
            }
        }

        public DecisionMaker()
        {
            if (decisionsList == null)
            {
                decisionsList = new LinkedList<Decision>();
            }
        }

        public void addDecision(string decisionName)
        {
            if (GivenDecisionExists(decisionName))
            {
                throw new Exception(string.Format("{0} exists already, please give it another name or delete the old one first", decisionName));
            }
            else
            {
                decisionsList.AddLast(new Decision(decisionName));
            }
        }

        public void deleteDecision(string decisionName)
        {
            if (GivenDecisionExists(decisionName))
            {
                decisionsList.Remove(decisionsList.LastOrDefault(d => d.DecisionName.ToLower() == decisionName.ToLower()));
            }
            else
            {
                throw new Exception(string.Format("{0} does not exist", decisionName));
            }
        }

        public void updateDecision(string oldDecisionName, Decision newDecision)
        {
            if (GivenDecisionExists(oldDecisionName))
            {
                decisionsList.LastOrDefault(d => d.DecisionName.ToLower() == oldDecisionName.ToLower()).DecisionName = newDecision.DecisionName;
            }
            else
            {
                throw new Exception(string.Format("{0} does not exist", oldDecisionName));
            }
        }

        public Decision GetDecisionByName(string decisionName)
        {
            return DecisionsList.FirstOrDefault(d => d.DecisionName.ToLower() == decisionName.ToLower());
        }

        public void madeDecision(string decisionName)
        {
            if (GivenDecisionExists(decisionName))
            {
                decisionsList.LastOrDefault(d => d.DecisionName.ToLower() == decisionName.ToLower()).DecisionMadeTime = DateTime.Now;
            }
            else
            {
                throw new Exception(string.Format("{0} does not exist", decisionName));
            }
        }

        private bool GivenDecisionExists(string decisionName)
        {
            return decisionsList.Any(d => d.DecisionName.ToLower() == decisionName.ToLower());
        }
    }
}
