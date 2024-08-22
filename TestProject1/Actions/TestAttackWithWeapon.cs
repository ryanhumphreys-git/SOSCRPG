using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Actions;
using Engine.Factories;
using Engine.Models;
using NUnit.Framework;

namespace TestEngine.Actions
{
    public class TestAttackWithWeapon
    {
        [Test]
        public void Test_Constructor_GoodParameters()
        {
            GameItem pointyStick = ItemFactory.CreateGameItem(1001);
            AttackWithWeapon attackWithWeapon = new AttackWithWeapon(pointyStick, 1, 5);
            Assert.IsNotNull(attackWithWeapon);
        }

        [Test]
        public void Test_Constructor_ItemIsNotAWeapon()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                GameItem granolaBar = ItemFactory.CreateGameItem(2001);
                AttackWithWeapon attackWithWeapon = new AttackWithWeapon(granolaBar, 1, 5);
            });
        }

        [Test]
        public void Text_Constructor_MinimumDamageLessThanZero()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                GameItem pointyStick = ItemFactory.CreateGameItem(1001);
                AttackWithWeapon attackWithWeapon = new AttackWithWeapon(pointyStick, -1, 5);
            });
        }

        [Test]
        public void Test_Constructor_MaximumDamageLessThanMinimumDamage()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                GameItem pointyStick = ItemFactory.CreateGameItem(1001);
                AttackWithWeapon attackWithWeapon = new AttackWithWeapon(pointyStick, 2, 1);
            });
        }
    }
}
