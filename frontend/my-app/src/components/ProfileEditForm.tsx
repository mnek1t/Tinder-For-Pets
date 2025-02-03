import { useState } from "react";
import { ProfileDetailsData, UpdateAnimalProfileRequest} from "../api/profileApi";
import { CircularProgress, FormControlLabel } from '@mui/material';
import FileUploader from "./FileUploader";
import CustomSwitch from "./CustomSwitch";
import LocationSuggestion from "../components/profile/LocationSuggestion"; 
import { useNavigate } from "react-router-dom";
import LoadButton from "./LoadButton";

interface ProfileEditFormProps {
    profileDetails : ProfileDetailsData | null,
    animalTypes: Array<{ id: number; typeName: string }>;
    breeds: Array<{ id: number; breedName: string }>;
    handleClickEditProfileCard : () => void;
    handleToggleSwitch: (event: React.ChangeEvent<HTMLInputElement>) => void;
    handleImageUpdate: (file: File) => Promise<any>;
    handleProfileUpdate: (request: UpdateAnimalProfileRequest) => Promise<any>;
}

const ProfileEditForm = (props: ProfileEditFormProps) => {
  const [formData, setFormData] = useState({
    name: props.profileDetails?.profile.name || "",
    dateOfBirth: props.profileDetails?.profile.dateOfBirth || "",
    description: props.profileDetails?.profile.description || "",
    typeId: props.animalTypes.find(type => type.typeName === props.profileDetails?.animal.type)?.id || 1,
    breedId: props.breeds.find(breed => breed.breedName === props.profileDetails?.animal.breed)?.id || 30,
    isVaccinated: props.profileDetails?.profile.isVaccinated || false,
    isSterilized: props.profileDetails?.profile.isSterilized || false,
    city: props.profileDetails?.profile.city,
    country: props.profileDetails?.profile.country,
  });
  const [profileImage, setProfileImage] = useState<File>();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate()
    function handleImageUpload(event : React.ChangeEvent<HTMLInputElement>) {
      const { files } = event.target;
      if (files && files[0]) {
          console.log("Image uploaded:", files[0]);
          setProfileImage(files[0]);
      }
    }

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
      const { name, value } = e.target;
      setFormData((prev) => ({
          ...prev,
          [name]: value,
      }));
  };

  const handleSwitchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      props.handleToggleSwitch(e);
      const { name, checked } = e.target;
      setFormData((prev) => ({
          ...prev,
          [name]: checked,
      }));
  };

  const handleContentEditableChange = (e: React.FocusEvent<HTMLHeadingElement | HTMLParagraphElement>, key: string) => {
      const value = e.currentTarget.textContent || "";
      setFormData((prev) => ({
          ...prev,
          [key]: value,
      }));
  };

  const handleLocationSelect = (city: string, country: string) => {
    setFormData(prevState => ({
        ...prevState,
        city: city,
        country: country,
    }));
};

    function createImageFileFromBase64() {
        const base64 = props.profileDetails?.images?.[0]?.imageData || '';
        const mimeType = props.profileDetails?.images?.[0]?.imageFormat || '';
        const binaryData = atob(base64);
        const byteArray = new Uint8Array(binaryData.length);

        for (let i = 0; i < binaryData.length; i++) {
            byteArray[i] = binaryData.charCodeAt(i);
        }
        return new File([byteArray], 'profileImage', { type: mimeType });
    }

    const handleSubmit = (e: React.FormEvent) => {
      e.preventDefault();
      console.log("Form to be submitted:", formData);
      console.log("Image to be submitted:", profileImage);
      const tasks: Promise<any>[] = [];
      if(profileImage) {
        tasks.push(props.handleImageUpdate(profileImage));
      }
      const request = {...formData, sexId: 1};
      tasks.push(props.handleProfileUpdate(request));
      setLoading(true)
      Promise.all(tasks)
      .then(() => {
        props.handleClickEditProfileCard();
        navigate('/app/profile');
      })
      .catch((error) => {
        console.error("Error occurred while updating profile:", error);
      })
      .finally(() => setLoading(false));
    };
    
    console.log(formData);
    return(
        <>
            <form onSubmit={handleSubmit}>
              <div className="profile-card">
                <FileUploader handleFileUpload={handleImageUpload} maxSizeMB="10" accept="image/*" initialImage={createImageFileFromBase64()}/>
                  <div style={{display:'flex',  justifyContent:'space-around', alignItems:'center', width:'100%'}}>
                    <h1 style={{width:'60%', wordBreak:'break-word'}} className="edit-profile-card__name" contentEditable suppressContentEditableWarning onBlur={(e) => handleContentEditableChange(e, "name")}>
                      {props.profileDetails?.profile.name}
                    </h1>

                    <input type="date" name="dateOfBirth" value={formData.dateOfBirth} className="profile-card__dob" onChange={handleInputChange}/>
                  </div>

                  <div style={{display:'flex',  justifyContent:'space-around', alignItems:'center'}}>
                    <div>
                      <label htmlFor="typeId">Animal Type</label>
                      <select name="typeId" value={formData.typeId} className="create-profile-form__animal-type" onChange={handleInputChange} required>
                      {props.animalTypes.map((type) => (
                          <option key={type.id} value={type.id}>
                              {String(type.typeName).charAt(0).toUpperCase() + String(type.typeName).slice(1)}
                          </option>
                      ))}
                      </select>
                    </div>

                    <span>,</span>

                    <div>
                      <label htmlFor="breedId">Breed</label>
                      <select name="breedId" value={formData.breedId} className="create-profile-form__breed" onChange={handleInputChange} required>
                          {props.breeds.map((breed) => (
                              <option key={breed.id} value={breed.id}>
                                  {breed.breedName}
                              </option>
                          ))}
                      </select>
                    </div>
                  </div>

                  <p contentEditable suppressContentEditableWarning onBlur={(e) => handleContentEditableChange(e, "description")}>
                    {props.profileDetails?.profile.description}
                  </p>

                  <div style={{padding:'10px'}}>
                    <LocationSuggestion onSelectLocation={handleLocationSelect} defaultValue={`${formData.city}, ${formData.country}`}/>
                  </div>

                  
                  <FormControlLabel
                    control={
                      <CustomSwitch name="isVaccinated" checked={props.profileDetails?.profile.isVaccinated} onChange={handleSwitchChange}/>
                    }
                    label="Vacinated:"
                    labelPlacement="start"
                    sx={{
                      fontFamily: "'Karla', sans-serif", 
                      fontSize: "14px",                 
                      ".MuiFormControlLabel-label": {
                        fontFamily: "'Karla', sans-serif", 
                      },
                    }}
                  />
                  <FormControlLabel
                    control={
                      <CustomSwitch name="isSterilized" checked={props.profileDetails?.profile.isSterilized} onChange={handleSwitchChange}/>
                    }
                    label="Sterialized:"
                    labelPlacement="start"
                    sx={{
                      fontFamily: "'Karla', sans-serif", 
                      fontSize: "14px",                  
                      ".MuiFormControlLabel-label": {
                        fontFamily: "'Karla', sans-serif", 
                      },
                    }}
                  />
                  {/* <button type="submit" name="update-profile-btn" className="edit-profile-btn">Save</button> */}
                  <LoadButton loading={loading} innertText="Save" />
                  <button type="button" name="cancel-update-profile-btn" className="edit-profile-btn" onClick={props.handleClickEditProfileCard}>Cancel</button>
              </div>
            </form>
        </>
    );
}

export default ProfileEditForm;