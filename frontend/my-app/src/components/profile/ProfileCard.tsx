import { getProfileDetails, ProfileDetailsData } from "../../api/profileApi";
import { useState, useEffect } from "react";
export default function ProfileCard()
{
    const [profileDetails, setProfileDetails] = useState<ProfileDetailsData>({
        name: "",
        description: "",
        age: 0,
        sex: "",
        type: "",
        breed: "",
        files: {
            imageData: "",
            imageFormat : ""
        },
        isVaccinated: false,
        isSterilized: false,
    });
    const [isDataReceived, setIsDataReceived] = useState<boolean>(false);
    useEffect(() => {
        getProfileDetails()
        .then(data => {
            console.log("response", data)
            setProfileDetails({
                name: data.profile?.name || "",
                description: data.profile?.description || "",
                age: data.profile?.age || 0,
                type: data.animal?.animalType || "",
                breed: data.animal?.breed || "",
                files: {
                    imageData: data.images[0]?.imageData,
                    imageFormat : data.images[0]?.imageFormat,
                },
                sex: data.profile?.sex || "",
                isVaccinated: data.profile?.isVaccinated || false,
                isSterilized: data.profile?.isSterilized || false,
            });
            setIsDataReceived(true);
        })
        .catch(error => {
            setIsDataReceived(false);
        })
    }, [])
    console.log("prifledata", profileDetails)
    return(
        <>
            {isDataReceived && <div className="profile-card">
                <img className="animal-image" src={`data:${profileDetails?.files.imageFormat};base64,${profileDetails?.files.imageData}`} alt="Animal"/>
                <h1>{profileDetails?.name}, {profileDetails?.age}</h1>
                <p className="profile-card__title">{profileDetails?.type}, {profileDetails?.breed}</p>
                <p>{profileDetails?.description}</p>
                <div className="medical-profile-data">
                    <label htmlFor="is_vaccinated">Is Vacinated :</label>
                    <input id="is_vaccinated" type="checkbox" checked={profileDetails?.isVaccinated}/>
                </div>
                <div className="medical-profile-data">
                    <label htmlFor="is_sterilized">Is Sterilized : </label>
                    <input id="is_sterilized" type="checkbox" checked={profileDetails?.isSterilized}/>
                </div>
                <button></button>
            </div>
        }
        </>
        
    );
}