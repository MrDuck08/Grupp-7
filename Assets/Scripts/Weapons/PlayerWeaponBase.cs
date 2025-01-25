using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum WeaponState
{
    Sword = 0,
    Axe = 1,
    Total
}

public class PlayerWeaponBase : MonoBehaviour
{
    #region Mouse

    [SerializeField] Camera cam;

    Vector2 mousePos;

    Rigidbody2D rb;

    [SerializeField] Transform playerTransform;

    public float WhereToLookOfset = 0;

    #endregion

    #region Change Weapon

    public WeaponBase[] avilableWeapons = new WeaponBase[(int)WeaponState.Total];
    public WeaponBase currentWeapon = null;


    float mouseAxisBreakpoin = 1.0f;
    float ScollWhellDelta = 0.0f;


    public bool swordActive = false;
    public bool axeActive = false;

    #endregion


    PlayerAxe playerAxe;
    PlayerSword playerSword;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerAxe = FindAnyObjectByType<PlayerAxe>();
        playerSword = FindAnyObjectByType<PlayerSword>();

        int currentWeaponIndex = (int)currentWeapon.weaponType;
        WeaponSwapAnimation(currentWeaponIndex);

    }


    void Update()
    {

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);


        HandleWeaponSwap();

        foreach (WeaponBase weapon in avilableWeapons)
        {
            if(weapon != currentWeapon)
            {
                weapon.gameObject.SetActive(false);
            }

        }

        currentWeapon.gameObject.SetActive(true);

        if (currentWeapon.weaponType == WeaponState.Axe)
        {
            axeActive = true;

            swordActive = false;
        }

        if (currentWeapon.weaponType == WeaponState.Sword)
        {
            swordActive = true;

            axeActive = false;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {

            //if (playerAxe)
            //{
                
            //    currentWeapon = avilableWeapons[((int)WeaponState.Axe)];
            //    currentWeapon.weaponType = WeaponState.Axe;

            //    swordActive = true;

            //    axeActive = false;

            //}
            //else
            //{
            //    currentWeapon.weaponType = WeaponState.Sword;
            //    currentWeapon = avilableWeapons[((int)WeaponState.Sword)];
            //    swordActive = false;

            //    axeActive = true;

            //}

        }
    }

    private void FixedUpdate()
    {

        int CurrenWeaponIndex = (int)currentWeapon.weaponType;

        Vector2 lookDirection = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        if(angle > 90 && angle < 180 || angle < -90 && angle > -180 && CurrenWeaponIndex == (int)WeaponState.Axe) // Kollar Om Yxan Är På Höger Sida
        {
            playerAxe.rightSideAxe = false;
            playerAxe.SideSwitch();
        }
        if (angle < 90 && angle > -90 && CurrenWeaponIndex == (int)WeaponState.Axe) // Kollar Om Yxan Är På Vänster Sida
        {
            playerAxe.rightSideAxe = true;
            playerAxe.SideSwitch();
        }

        if(CurrenWeaponIndex == (int)WeaponState.Sword)
        {
            WhereToLookOfset = 0;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, angle + WhereToLookOfset);
    }

    #region Change Scenes

    public void ChangeScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Left game");
        Application.Quit();
    }

    public void LoadNextScene()
    {
        int nextSceneIndex = LoopBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(nextSceneIndex);
    }

    private int LoopBuildIndex(int buildIndex)
    {
        if (buildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            buildIndex = 0;
        }

        return buildIndex;
    }

    #endregion

    #region Change Weapon


    private void HandleWeaponSwap()
    {

        ScollWhellDelta += Input.mouseScrollDelta.y;
        if (Mathf.Abs(ScollWhellDelta) > mouseAxisBreakpoin)
        {
            int SwapDirection = (int)Mathf.Sign(ScollWhellDelta);
            ScollWhellDelta -= SwapDirection * mouseAxisBreakpoin;

            int CurrenWeaponIndex = (int)currentWeapon.weaponType;
            CurrenWeaponIndex += SwapDirection;

            if (CurrenWeaponIndex < 0)
            {
                CurrenWeaponIndex = (int)WeaponState.Total + CurrenWeaponIndex;
                // Byter Till Första Vapnet Om Den går -1
            }
            if (CurrenWeaponIndex >= (int)WeaponState.Total)
            {
                CurrenWeaponIndex = 0;
                // Byter Tillbacka Till Första Vapnet Om Du Går Över Max Antal Vapen
            }
            WeaponSwapAnimation(CurrenWeaponIndex);

        }
    }

    private void WeaponSwapAnimation(int currentWeaponIndex)
    {
        foreach (var weapon in avilableWeapons)
        {
            weapon.gameObject.SetActive(false);
            
        }
        currentWeapon.stopAttacking = true;
        currentWeapon = avilableWeapons[currentWeaponIndex];
        currentWeapon.gameObject.SetActive(true);

        if(currentWeaponIndex == (int)WeaponState.Axe)
        {
            playerAxe = FindAnyObjectByType<PlayerAxe>();

            playerAxe.ResetAttack();
        }

        if (currentWeaponIndex == (int)WeaponState.Sword)
        {
            playerSword = FindAnyObjectByType<PlayerSword>();

            playerSword.ResetAttack();
        }
    }

    #endregion

}
