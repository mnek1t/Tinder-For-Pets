import "../styles/profile.css";
import CustomSwitch from "../CustomSwitch";
import { FormControlLabel, Typography } from "@mui/material";
import CustomSlider from "./CustomSlider";
export interface UserPreferencesProps {
    distance: number;
    onDistanceChange: (value: number) => void;
    showPetsInRange: boolean;
    onToggleShowPets: () => void;
    onLightMode: () => void;
    onDarkMode: () => void;
    handleLogout: (event : React.MouseEvent<HTMLButtonElement>) => void;
    handleDeleteAccount: (event : React.MouseEvent<HTMLButtonElement>) => void;
  }

export default function UserPreferences(props : UserPreferencesProps) {
    return(
        <>
            <h3 className="sidebar-title">ACCOUNT SETTINGS</h3>
            <button className="preference-btn">Manage Payment Account</button>
            <h3 className="sidebar-title">DISCOVERY SETTINGS</h3>
            <div>
                <Typography gutterBottom 
                    sx={{
                        fontFamily: "'Karla', sans-serif", 
                        fontSize: "14px",                  
                        ".MuiFormControlLabel-label": {
                        fontFamily: "'Karla', sans-serif", 
                        },
                    }}>
                        Distance Preference {props.distance}km
                </Typography>
                <CustomSlider aria-label="ios slider" value={props.distance}  valueLabelDisplay="on" onChange={(e, value) => props.onDistanceChange(value as number)}/>

                <div className="toggler-preference">
                        <FormControlLabel
                            control={
                                <CustomSwitch name="isSterilized" checked={props.showPetsInRange} onChange={props.onToggleShowPets}/>
                            }
                            label="Only show pets in this range"
                            labelPlacement="start"
                            sx={{
                                fontFamily: "'Karla', sans-serif", 
                                fontSize: "14px",                  
                                ".MuiFormControlLabel-label": {
                                    fontFamily: "'Karla', sans-serif", 
                                },
                            }}
                        />
                </div>    
            </div>
            
            <h3 className="sidebar-title">APPEARANCE</h3>
            <button className="preference-btn" onClick={props.onLightMode}>Light Mode</button>
            <button className="preference-btn" onClick={props.onDarkMode}>DarkMode</button>

            <h3 className="sidebar-title">LEGAL</h3>
            
            <div className="legal-section">
                <a href="#Privacy Settings">Privacy Settings</a>
                <a href="#Cookie Policy">Cookie Policy</a>
                <a href="#Privacy Policy">Privacy Policy</a>
                <a href="#Terms of Service">Terms of Service</a>
            </div>
            <br/>
            <button className="preference-btn" onClick={props.handleLogout}>Log out</button>
            <button className="preference-btn" onClick={props.handleDeleteAccount}>Delete Account</button>
        </>
    );
}