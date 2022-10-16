import React from 'react';
import { ChatBoxAppState, ChatMessage } from '../components/contracts';

export const ChatBoxesContext = React.createContext<ChatBoxAppState | undefined>({messages:[], setMessages:(messages:ChatMessage[]) => {}});