using System;
using System.Collections.Generic;
using Items;
using Units.Boids;
using Units.Logic;
using UnityEngine;

namespace Environment.Workplaces
{
    public class WildPlot : MonoBehaviour, IWorkplace
    {
        public ResourceDataFactory ResourceDataFactory;
        
        public int workerLimit = 3;
        public int stashLimit = 10;
        public int cropsLimit = 5;

        public float neediness = 1f; //growth = (W/maxW) ^ neediness
        
        [HideInInspector] public List<WorkerLogic> workers = new List<WorkerLogic>();
        
        private List<Crop> _crops = new List<Crop>();
        private List<ResourceData> _resourceStash = new List<ResourceData>();

        private float _growthTimer = 0f;

        private void FixedUpdate()
        {
            RefillCrops();
            
            _growthTimer += Time.fixedDeltaTime;
            if (_growthTimer >= 1f)
            {
                HandleGrowth();
                _growthTimer = 0f;
            }
        }

        private void HandleGrowth()
        {
            float growthEfficiency = Mathf.Pow(workers.Count / (float)workerLimit, neediness);

            List<Crop> cropsToRemove = new List<Crop>();

            foreach (Crop crop in _crops)
            {
                crop.Grow(growthEfficiency);
                if (crop.Maturity >= 1 && _resourceStash.Count < stashLimit)
                {
                    _resourceStash.Add(crop.GiveResource());
                    cropsToRemove.Add(crop);
                    Debug.Log("Stashing crop");
                }
            }

            foreach (Crop crop in cropsToRemove)
            {
                Debug.Log("Removing crop");
                _crops.Remove(crop);
            }
        }

        private void RefillCrops()
        {
            while (_crops.Count < cropsLimit)
            {
                ResourceData newResData = ResourceDataFactory.GenerateNewResource();
                Crop newCrop = new Crop(newResData);
                _crops.Add(newCrop);
                Debug.Log("adding new crop");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            WorkerBoid workerBoid = other.GetComponent<WorkerBoid>();
            if (workerBoid != null && workerBoid.TargetWorkplace == transform)
            {
                workerBoid.SetBoidState(WorkerBoidState.Working);
                AddWorker(workerBoid);
            }
        }

        public void AddWorker(WorkerBoid worker)
        {
            Debug.Log("Adding worker");
            WorkerLogic workerLogic = worker.GetComponent<WorkerLogic>();
            if (workerLogic != null)
            {
                workers.Add(workerLogic);
            }
        }

        public void RemoveWorker(WorkerBoid worker)
        {
            WorkerLogic workerLogic = worker.GetComponent<WorkerLogic>();
            if (workerLogic != null)
            {
                workers.Remove(workerLogic);
            }
        }
    }
}
