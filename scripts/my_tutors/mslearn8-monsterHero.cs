int monsterHealth = 10;
int heroHealth = 10;
int monsterHit;
int heroHit;
Random random = new();

do
{
  heroHit = random.Next(1, 11);
  monsterHealth -= heroHit;
  Console.WriteLine($"Monster was damaged and lost {heroHit} and now has {monsterHealth} health.");
  if (monsterHealth <= 0)
    Console.WriteLine("Hero wins!");
  else
  {
    monsterHit = random.Next(1, 11);
    heroHealth -= monsterHit;
    Console.WriteLine($"Hero was damaged and lost {monsterHit} and now has {heroHealth} health.");
    if (heroHealth <= 0)
    Console.WriteLine("Monster wins!");
  }

} while (monsterHealth > 0 && heroHealth > 0);
