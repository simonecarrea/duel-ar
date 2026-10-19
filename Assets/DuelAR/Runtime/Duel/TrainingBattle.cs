using System;

namespace DuelAR.Duel
{
    // Training keeps monsters after battle; only attack-position life damage is resolved.
    public sealed class TrainingBattle
    {
        public int DragonLife { get; private set; } = 4000;
        public int MageLife { get; private set; } = 4000;
        public bool DragonTurn { get; private set; } = true;
        public bool Finished => DragonLife == 0 || MageLife == 0;
        public int ResolveAttack()
        {
            if (Finished) throw new InvalidOperationException("Duello terminato.");
            // 3000 vs 2500: Mago Nero loses the difference regardless of who attacks.
            const int damage = 3000 - 2500;
            MageLife = Math.Max(0, MageLife - damage);
            DragonTurn = !DragonTurn;
            return damage;
        }
        public void Reset() { DragonLife = MageLife = 4000; DragonTurn = true; }
    }
}
