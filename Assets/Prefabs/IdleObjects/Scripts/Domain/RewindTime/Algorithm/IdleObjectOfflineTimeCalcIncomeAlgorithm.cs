using System;
using System.Collections.Generic;
using System.Linq;
using Orego.Util;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Tools;

namespace SinSity.Domain
{
    /// <summary>
    /// Считает доход за оффлайн время (начинает с прогресса последнего цикла).
    /// </summary>
    public static class IdleObjectOfflineTimeCalcIncomeAlgorithm
    {
        /// <param name="oneCycleIncomeBase">Доход за цикл</param>
        /// <param name="lastCycleProgressSeconds">Прогресс секунд с последнего цикла</param>
        /// <param name="oneCycleDurationSeconds">Период цикла</param>
        /// <param name="offlineSeconds">Прошло секунд с последнего запуска онлайн</param>
        /// <param name="constCoefficients">Простые коеффициенты дохода</param>
        /// <param name="timeCoefficients">Временные коеффициенты дохода</param>
        public sealed class Params
        {
            public BigNumber oneCycleIncomeBase { get; set; }

            public float lastCycleProgressSeconds { get; set; }

            public float oneCycleDurationSeconds { get; set; }

            public long offlineSeconds { get; set; }

            public List<Coefficient> constCoefficients { get; set; }

            public List<TimeCoefficient> timeCoefficients { get; set; }

            public IdleObject idleObject { get; set; }
        }

        public static BigNumber CalcIncomeByPassedOfflineTime(IdleObject idleObject, long offlineSeconds)
        {
            var idleObjectState = idleObject.state;
            var constCoefficients = idleObjectState.incomeConstMultiplicator.GetMultipliers();
            constCoefficients.Add(new Coefficient(idleObjectState.level));
            constCoefficients.Add(new Coefficient(idleObjectState.localMultiplicatorDynamic));
            var algorithmParams = new IdleObjectOfflineTimeCalcIncomeAlgorithm.Params
            {
                constCoefficients = constCoefficients,
                timeCoefficients = idleObjectState.incomeTimeMultiplicator.GetMultipliers(),
                lastCycleProgressSeconds = idleObject.state.progressInTime,
                offlineSeconds = offlineSeconds,
                oneCycleDurationSeconds = idleObject.state.incomePeriod,
                oneCycleIncomeBase = new BigNumber(idleObject.info.incomeDefault.ToString(BigNumber.FORMAT_FULL)),
                idleObject = idleObject
            };

            return CalcIncomeByPassedOfflineTime(algorithmParams);
        }

        public static BigNumber CalcIncomeByPassedOfflineTime(Params parameters)
        {
            var totalConstCoefficient = GetTotalConstCoefficient(parameters.constCoefficients);
            var totalConstIncome = parameters.oneCycleIncomeBase * totalConstCoefficient;
            var restOfflineTimeSeconds = (long) (parameters.offlineSeconds + parameters.lastCycleProgressSeconds);
            var restTimeCoefficients = parameters.timeCoefficients.ToList();
            if (restTimeCoefficients.IsEmpty())
            {
                return CalcIncomeByCyclesSimple(parameters, restOfflineTimeSeconds, totalConstIncome);
            }

            return CalIncomeByCyclesWithTime(
                parameters,
                restTimeCoefficients,
                restOfflineTimeSeconds,
                totalConstIncome
            );
        }

        private static BigNumber CalcIncomeByCyclesSimple(
            Params parameters,
            long restOfflineTimeSeconds,
            BigNumber totalConstIncome
        )
        {
            var oneCycleDurationSeconds = parameters.oneCycleDurationSeconds;
            var passedCycleCount = Mathf.FloorToInt(restOfflineTimeSeconds / oneCycleDurationSeconds);
            var totalIncome = totalConstIncome * passedCycleCount;
            return totalIncome;
        }

        private static BigNumber CalIncomeByCyclesWithTime(
            Params parameters,
            List<TimeCoefficient> restTimeCoefficients,
            long restOfflineTimeSeconds,
            BigNumber totalConstIncome
        )
        {
            var totalIncome = new BigNumber(0);
            var oneCycleDurationSeconds = parameters.oneCycleDurationSeconds;
            while (restTimeCoefficients.IsNotEmpty())
            {
                var minTimeCoefficient = FindMinTimeCoefficient(restTimeCoefficients);
                var minTimeCoefficientSeconds = minTimeCoefficient.passedTimeSeconds;
                var totalTimeCoefficient = GetTotalTimeCoefficient(restTimeCoefficients);
                if (restOfflineTimeSeconds <= minTimeCoefficientSeconds)
                {
                    var passedCycleCount = restOfflineTimeSeconds / oneCycleDurationSeconds;
                    var cyclesIncome = totalConstIncome * passedCycleCount;
                    var timeCyclesIncome = cyclesIncome * totalTimeCoefficient;
                    totalIncome += timeCyclesIncome;
                    return totalIncome;
                }
                else
                {
                    var passedCycleCount = minTimeCoefficientSeconds / oneCycleDurationSeconds;
                    var cyclesIncome = totalConstIncome * passedCycleCount;
                    totalIncome += cyclesIncome * totalTimeCoefficient;
                    var cyclesTime = passedCycleCount * oneCycleDurationSeconds;
                    restOfflineTimeSeconds -= (long) cyclesTime;
                    restTimeCoefficients.Remove(minTimeCoefficient);
                    foreach (var otherTimeCoefficient in restTimeCoefficients)
                    {
                        otherTimeCoefficient.passedTimeSeconds -= (long) cyclesTime;
                    }
                }
            }

            var simpleConstIncome = CalcIncomeByCyclesSimple(parameters, restOfflineTimeSeconds, totalConstIncome);
            totalIncome += simpleConstIncome;
            return totalIncome;
        }

        private static double GetTotalConstCoefficient(List<Coefficient> constCoefficients)
        {
            double totalMultiplier = 1;
            foreach (var constCoefficient in constCoefficients)
            {
                var constMultplier = constCoefficient.value;
                totalMultiplier *= constMultplier;
            }

            return totalMultiplier;
        }

        private static double GetTotalTimeCoefficient(List<TimeCoefficient> timeCoefficients)
        {
            double totalMultiplier = 1;
            foreach (var timeCoefficient in timeCoefficients)
            {
                var constMultplier = timeCoefficient.value;
                totalMultiplier *= constMultplier;
            }

            return totalMultiplier;
        }

        private static TimeCoefficient FindMinTimeCoefficient(List<TimeCoefficient> coefficients)
        {
            var coefficientsCount = coefficients.Count;
            if (coefficientsCount <= 0)
            {
                throw new Exception("Coefficients not found!");
            }

            var requiredCoefficient = coefficients[0];
            if (coefficientsCount == 1)
            {
                return requiredCoefficient;
            }

            for (var i = 1; i < coefficientsCount; i++)
            {
                var currentCoefficient = coefficients[i];
                if (currentCoefficient.passedTimeSeconds < requiredCoefficient.passedTimeSeconds)
                {
                    requiredCoefficient = currentCoefficient;
                }
            }

            return requiredCoefficient;
        }
    }
}