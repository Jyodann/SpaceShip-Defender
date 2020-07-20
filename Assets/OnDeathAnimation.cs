using UnityEngine;

public class OnDeathAnimation : MonoBehaviour
{
    [SerializeField] private GameObject smallExplosion;
    [SerializeField] private GameObject bigExplosion;
    [SerializeField] private GameObject ufoExplosion;
    public enum ExplosionTypes
    {
        BigExplosion, SmallExplosion, UFOExplosion
    }

    private ExplosionTypes explosionType;
    public void MakeExplosion(Transform explosionLocation, ExplosionTypes explosionTypes)
    {
        switch (explosionTypes)
        {
            case ExplosionTypes.BigExplosion:
                Destroy(Instantiate(bigExplosion, explosionLocation.position, Quaternion.identity), 2f);
                break;
            case ExplosionTypes.SmallExplosion:
                Destroy(Instantiate(smallExplosion, explosionLocation.position, Quaternion.identity), 2f);
                break;
            case ExplosionTypes.UFOExplosion:
                Destroy(Instantiate(ufoExplosion, explosionLocation.position, Quaternion.identity), 2f);
                break;
        }
    }
}