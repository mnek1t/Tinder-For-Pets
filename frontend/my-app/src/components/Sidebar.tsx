import "../styles/profile.css"
import EmptyHeader from "./EmptyHeader";
import UserPreferences, { UserPreferencesProps } from "./UserPreferences"
import TabSelection from "./TabSelection";
interface SidebarProps extends Partial<UserPreferencesProps> { // partial means optional props
    isUserPreferences: boolean
} 

export default function Sidebar({
    isUserPreferences = true,
    distance,
    onDistanceChange,
    showPetsInRange,
    onToggleShowPets,
    onLightMode,
    onDarkMode,
    handleLogout,
    handleDeleteAccount,
} : SidebarProps) {
   
    return(
        <div className="sidebar">
            <EmptyHeader/>
            { isUserPreferences ? <UserPreferences 
                distance = {distance!}
                onDistanceChange = {onDistanceChange!}
                showPetsInRange = {showPetsInRange!}
                onToggleShowPets = {onToggleShowPets!}
                onLightMode = {onLightMode!}
                onDarkMode = {onDarkMode!}
                handleLogout = {handleLogout!}
                handleDeleteAccount = {handleDeleteAccount!}
            /> : <TabSelection/>}
            
        </div>
        );
}