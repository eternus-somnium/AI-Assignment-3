using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tank : Entity 
{
	public bool active;
	public Vector3 
		position,
		hangerPosition;
	public Quaternion
		hangerRotation;
	public GameObject panel;
	public Color color;
	public float 
		weight,
		speed,
		heading = 0;

	public int 
		bounty = 1, 
		funds = 0;

	public GameObject
		bodySchematic,
		trackSchematic,
		mainWeaponSchematic,
		secondaryWeaponSchematic,
		accessorySchematic;
    GameObject
        body,
        track,
        mainWeapon,
        secondaryWeapon,
        accessory;
	Spawner s;

    public PathNode moveTarget = null;

    private AI ai = null;

    //Indicator for whether or not tank is currently safe in hanger
    public bool safe;

	// Use this for initialization
	public void Awake () 
	{
		DontDestroyOnLoad(gameObject);
	}

	public void Start()
	{
		s=GameObject.Find("GameManager").GetComponent<Spawner>();

        ai = gameObject.GetComponent<AI>();

        safe = true;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		if(active)
		EntityUpdate();
		heading = Mathf.RoundToInt(gameObject.transform.rotation.eulerAngles.y);
		position = gameObject.transform.position;
	}

	public void FireMain()
	{
		if(mainWeapon != null)
			mainWeapon.GetComponent<Weapon>().Fire();
	}

	public void FireSecondary()
	{
		if(secondaryWeapon != null)
			secondaryWeapon.GetComponent<Weapon>().Fire();
	}

	public void Move(float gas, float turn)
	{
		transform.Translate(Vector3.forward * gas * speed * Time.deltaTime);
		transform.Rotate(new Vector3(0, turn * 25 * Time.deltaTime, 0));
	}

	public void RotateTurret(float turretTurn)
	{
		mainWeapon.transform.Rotate(new Vector3(0, turretTurn * 25 * Time.deltaTime, 0));
	}

    public void RotateTurretTowards(GameObject obj)
    {
        mainWeapon.transform.LookAt(obj.transform.position);
    }

	public void startRound()
	{
		GetComponent<Rigidbody>().useGravity = true;
		active = true;
		weight = 0;

		//Build tank
		if(bodySchematic != null)
		{
			body = (GameObject) Instantiate(bodySchematic, transform.position + bodySchematic.transform.position, Quaternion.identity);
			body.transform.SetParent(transform);
			weight += body.GetComponent<Part>().weight;
		}
		if(trackSchematic != null)
		{
			track = (GameObject) Instantiate(trackSchematic, transform.position + trackSchematic.transform.position, Quaternion.identity);
			track.transform.SetParent(transform);
			weight += track.GetComponent<Part>().weight;
		}
		if(mainWeaponSchematic != null)
		{
			mainWeapon = (GameObject) Instantiate(mainWeaponSchematic, transform.position + mainWeaponSchematic.transform.position, Quaternion.identity);
			mainWeapon.transform.SetParent(transform);
			weight += mainWeapon.GetComponent<Part>().weight;
		}
		if(secondaryWeaponSchematic != null)
		{
			secondaryWeapon = (GameObject) Instantiate(secondaryWeaponSchematic, transform.position + secondaryWeaponSchematic.transform.position, Quaternion.identity);
			secondaryWeapon.transform.SetParent(transform);
			weight += secondaryWeapon.GetComponent<Part>().weight;
		}
		if(accessorySchematic != null)
		{
			accessory = (GameObject) Instantiate(accessorySchematic, transform.position + accessorySchematic.transform.position, Quaternion.identity);
			accessory.transform.SetParent(transform);
			weight += accessory.GetComponent<Part>().weight;
		}

		//Set stats
		maxHealth = body.GetComponent<Part>().attribute;

		speed = track.GetComponent<Part>().attribute / weight;

		panel = GameObject.Find("LeftPanel").GetComponent<LeftPanel>().AddUnitPanel();
		panel.GetComponentsInChildren<Image>()[1].color = color;

		//Refresh drivers
		GetComponent<Driver>().p = GameObject.Find("ArenaAI").GetComponent<PathFinding>();
	}

	public void EndRound()
	{
		GetComponent<Rigidbody>().useGravity = false;
		active = false;
		Destroy(body);
		Destroy(track);
		Destroy(mainWeapon);
		Destroy(secondaryWeapon);
		Destroy(accessory);
	}

	public void addKill(int addValue)
	{
		bounty += addValue;
		funds += addValue;
		setBountyBoardValue();
	}

	public void setBountyBoardValue()
	{
		panel.GetComponentInChildren<Text>().text = bounty.ToString();
	}

	public override void Death ()
	{
        if (ai != null)
        {
            ai.OnDeath();
        }

		s.SpawnTank(gameObject, false);
	}
}
