import React from 'react';
import { ChatBoxesContext } from '../../contexts';
import { ChatFieldProps } from '../contracts';

export const ChatField: React.FC<ChatFieldProps> = ({ author }) => {
    const inputRef = React.createRef<HTMLInputElement>();
    const contextData = React.useContext(ChatBoxesContext);

    const sendMessage = React.useCallback(() => {
        if (inputRef.current!.value?.length > 0) {
            contextData?.setMessages([...contextData?.messages, { content: inputRef.current!.value, author }]);
            inputRef.current!.value = '';
        }
    }, [author, inputRef, contextData]);

    return (
        <form data-testid="chatfield-container" className="ChatFieldContainer" onSubmit={(event) => event.preventDefault()}>
            <input ref={inputRef} data-testid="chatfield-input" placeholder="Enter your text here" />
            <button data-testid="chatfield-button" className="ChatFieldButton" onClick={sendMessage}>Ok</button>
        </form>
    )
}

export default ChatField;
ChatField.displayName = 'ChatField';