import { getProfileDetails, ProfileDetailsData } from "../../api/profileApi";
import { useState, useEffect } from "react";
export default function ProfileCard()
{
    const [profileDetails, setProfileDetails] = useState<ProfileDetailsData | null>(null);
    const [isDataReceived, setIsDataReceived] = useState<boolean>(false);
    useEffect(() => {
        getProfileDetails()
        .then(({ profile, animal, images }) => {
            setProfileDetails({
                animal: {
                  type: animal?.animalType || "", // Ensure the animal type is set properly
                  breed: animal?.breed || "", // Ensure breed is set properly
                },
                profile: {
                  id: profile?.id || "", // Ensure the profile id is set
                  name: profile?.name || "",
                  description: profile?.description || "",
                  age: profile?.age || 0,
                  sex: profile?.sex || "",
                  isVaccinated: profile?.isVaccinated || false,
                  isSterilized: profile?.isSterilized || false,
                },
                images: [
                  {
                    imageData: images[0]?.imageData || "", // Safely access imageData
                    imageFormat: images[0]?.imageFormat || "", // Safely access imageFormat
                  },
                ],
              });
            setIsDataReceived(true);
        })
        .catch(error => {
            setIsDataReceived(false);
            console.error('Error fetching profile details:', error);
        })
    }, [])

    return(
        <>
            {isDataReceived && <div className="profile-card">
                <img className="animal-image" src={`data:${profileDetails?.images[0].imageFormat};base64,${profileDetails?.images[0].imageData}`} alt="Animal"/>
                <h1 className="profile-card__name">{profileDetails?.profile.name}, {profileDetails?.profile.age}</h1>
                <p className="profile-card__title">{profileDetails?.animal.type}, {profileDetails?.animal.breed}</p>
                <p>{profileDetails?.profile.description}</p>
                <div className="medical-profile-data">
                    <label htmlFor="is_vaccinated">Is Vacinated :</label>
                    <input id="is_vaccinated" type="checkbox" checked={profileDetails?.profile.isVaccinated}/>
                </div>
                <div className="medical-profile-data">
                    <label htmlFor="is_sterilized">Is Sterilized : </label>
                    <input id="is_sterilized" type="checkbox" checked={profileDetails?.profile.isSterilized}/>
                </div>
                <button className="edit-profile-btn" onClick={() => console.log("This will call edit profile page")}>Edit Profile</button>
            </div>
        }
        </>
        
    );
}