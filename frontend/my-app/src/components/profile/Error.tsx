export default function Error({ error }: { error: Error }) {
    return (
        <div
            style={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                textAlign: 'center',
                padding: '8px', 
                backgroundColor: '#ffe6e6', 
                border: '1px solid red',
                borderRadius: '4px', 
                marginBottom: '12px', 
                maxWidth: '90%', 
            }}
        >
            <span style={{ fontSize: '14px', color: 'red' }}>{error.message}</span>
        </div>
    );
}
