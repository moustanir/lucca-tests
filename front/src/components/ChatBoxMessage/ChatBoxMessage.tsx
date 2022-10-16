import React from 'react';
import { ChatBoxMessageProps } from '../contracts';

export const ChatBoxMessage : React.FC<ChatBoxMessageProps> = ({message, id, reader}) =>{
    return (<p key={id} >{message.author === reader ? "Me" : message.author}: {message.content}</p>)
}

export default ChatBoxMessage;
ChatBoxMessage.displayName = 'ChatBoxMessage';