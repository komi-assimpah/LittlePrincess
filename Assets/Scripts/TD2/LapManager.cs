using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Linq;

public class LapManager : MonoBehaviour
{
    public List<Checkpoint> checkpoints;
    public int totalLaps = 3;
    public UIManager ui;
    public PodiumManager podiumManager;  // Référence au PodiumManager

    private List<PlayerRank> playerRanks = new List<PlayerRank>();
    private PlayerRank mainPlayerRank;

    public UnityEvent onPlayerFinished = new UnityEvent();

    // Liste des voitures dans la course
    public List<CarIdentity> raceCars = new List<CarIdentity>();

    // État de fin de course
    private bool raceFinished = false;

    // Liste pour stocker les positions finales
    private List<CarIdentity> finalRanking = new List<CarIdentity>();

    void Start()
    {
        foreach (CarIdentity carIdentity in GameObject.FindObjectsOfType<CarIdentity>())
        {
            Debug.Log(carIdentity.carName + " en piste");
            playerRanks.Add(new PlayerRank(carIdentity));
        }

        if (playerRanks.Count == 0)
        {
            Debug.LogError("Aucun joueur trouve ! Assurez-vous que chaque voiture possede un composant CarIdentity.");
            return;
        }

        ListenCheckpoints(true);

        mainPlayerRank = playerRanks.Find(player => player.identity.gameObject.tag == "Player");

        ui.UpdateLapText(mainPlayerRank.identity.carName + ": lap " + playerRanks[0].lapNumber + " / " + totalLaps);
    }

    private void ListenCheckpoints(bool subscribe)
    {
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (subscribe)
            {
                checkpoint.onCheckpointEnter.AddListener((car, cp) =>
                {
                    var carIdentity = car.GetComponent<CarIdentity>();
                    if (carIdentity != null)
                    {
                        Debug.Log($"Checkpoint triggered by: {carIdentity.carName}");
                        CheckpointActivated(carIdentity, checkpoint);
                    }
                    else
                    {
                        Debug.LogError($"CarIdentity component not found on GameObject: {car.name}");
                    }
                });
            }
            else
            {
                checkpoint.onCheckpointEnter.RemoveListener((car, cp) =>
                {
                    var carIdentity = car.GetComponent<CarIdentity>();
                    if (carIdentity != null)
                    {
                        CheckpointActivated(carIdentity, checkpoint);
                    }
                });
            }
        }
    }

    public void CheckpointActivated(CarIdentity car, Checkpoint checkpoint)
    {
        PlayerRank player = playerRanks.Find((rank) => rank.identity == car);
        if (checkpoints.Contains(checkpoint) && player != null)
        {
            if (player.hasFinished) return;

            int checkpointNumber = checkpoints.IndexOf(checkpoint);
            bool startingFirstLap = checkpointNumber == 0 && player.lastCheckpoint == -1;
            bool lapIsFinished = checkpointNumber == 0 && player.lastCheckpoint >= checkpoints.Count - 1;
            if (startingFirstLap || lapIsFinished)
            {
                player.lapNumber += 1;
                player.identity.lapNumber = player.lapNumber;
                player.lastCheckpoint = 0;

                if (player.lapNumber > totalLaps)
                {
                    player.hasFinished = true;
                    player.rank = playerRanks.FindAll(player => player.hasFinished).Count;

                    if (player.rank == 1)
                    {
                        Debug.Log(player.identity.carName + " won");
                        ui.UpdateLapText(player.identity.carName + " won");
                    }

                    if (player == mainPlayerRank)
                    {
                        ui.UpdateLapText("\nYou finished in " + mainPlayerRank.rank + " place");
                        onPlayerFinished.Invoke();

                        EndRace();
                    }
                    //CarFinishedRace(car);
                }
                else
                {
                    Debug.Log(player.identity.carName + ": lap " + player.lapNumber);
                    ui.UpdateLapText(player.identity.carName + ": lap " + player.lapNumber + " / " + totalLaps);
                }
            }
            else if (checkpointNumber == player.lastCheckpoint + 1)
            {
                player.lastCheckpoint += 1;
            }
        }
    }

    // Méthode appelée quand une voiture termine la course
    public void CarFinishedRace(CarIdentity car)
    {
        if (!finalRanking.Contains(car))
        {
            finalRanking.Add(car);
            Debug.Log($"{car.name} a terminé en position {finalRanking.Count}");

            // Si toutes les voitures ont terminé, on termine la course et on affiche le podium
            if (finalRanking.Count == raceCars.Count)
            {
                EndRace();
            }
        }
    }

    // Terminer la course et afficher le podium
    public void EndRace()
    {
        if (!raceFinished)
        {
            raceFinished = true;
            Debug.Log("Course terminée!");

            // Activer le Podium
            if (podiumManager != null)
            {
                podiumManager.gameObject.SetActive(true); // Assure que c'est actif
                podiumManager.ShowPodiumUI();
            }
        }
    }

    // Retourne le classement final (pour le podium)
    public List<CarIdentity> GetRaceResults()
    {
        return playerRanks
            .OrderBy(p => p.hasFinished ? p.rank : int.MaxValue) // ceux qui ont fini sont triés par rank
            .ThenByDescending(p => p.lapNumber)                   // puis ceux qui n'ont pas fini triés par progression
            .ThenByDescending(p => p.lastCheckpoint)
            .Select(p => p.identity)
            .ToList();
    }

}
